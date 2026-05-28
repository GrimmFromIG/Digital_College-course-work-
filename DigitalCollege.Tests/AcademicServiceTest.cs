using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.BLL.Services;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.Repositories;
using DigitalCollege.DAL.UnitOfWork;

namespace DigitalCollege.BLL.Tests
{
    public class AcademicServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Grade>> _gradeRepositoryMock;
        private readonly Mock<IRepository<Discipline>> _disciplineRepositoryMock;
        private readonly AcademicService _academicService;

        public AcademicServiceTests()
        {
            // Arrange
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gradeRepositoryMock = new Mock<IRepository<Grade>>();
            _disciplineRepositoryMock = new Mock<IRepository<Discipline>>();

            _unitOfWorkMock.Setup(u => u.Grades).Returns(_gradeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Disciplines).Returns(_disciplineRepositoryMock.Object);

            _academicService = new AcademicService(_unitOfWorkMock.Object);
        }

        [Fact]
        public void AssignGrade_ShouldThrowException_WhenDisciplineNotFound()
        {
            // Arrange
            var gradeDto = new GradeDto { DisciplineId = 999, TeacherId = 1, Value = 95, StudentId = 1 };
            _disciplineRepositoryMock.Setup(r => r.GetById(999)).Returns((Discipline)null);

            // Act & Assert
            var exception = Assert.Throws<BusinessLogicException>(() => _academicService.AssignGrade(gradeDto));
            Assert.Equal("Дисципліну не знайдено.", exception.Message);
        }

        [Fact]
        public void AssignGrade_ShouldThrowException_WhenTeacherDoesNotTeachDiscipline()
        {
            // Arrange
            int disciplineId = 1;
            var gradeDto = new GradeDto { DisciplineId = disciplineId, TeacherId = 5, Value = 90, StudentId = 1 }; // Запит від викладача з ID 5

            var existingDiscipline = new Discipline { Id = disciplineId, Name = "Математика", TeacherId = 2 }; // Веде викладач з ID 2
            _disciplineRepositoryMock.Setup(r => r.GetById(disciplineId)).Returns(existingDiscipline);

            // Act & Assert
            var exception = Assert.Throws<BusinessLogicException>(() => _academicService.AssignGrade(gradeDto));
            Assert.Equal("Ви не можете виставляти оцінки по чужій дисципліні.", exception.Message);
        }

        [Fact]
        public void AssignGrade_ShouldAddGradeSuccessfully_WhenDataIsValid()
        {
            // Arrange
            int disciplineId = 2;
            int teacherId = 3;
            var gradeDto = new GradeDto { DisciplineId = disciplineId, TeacherId = teacherId, Value = 100, StudentId = 10 };

            var existingDiscipline = new Discipline { Id = disciplineId, Name = "Програмування", TeacherId = teacherId };
            _disciplineRepositoryMock.Setup(r => r.GetById(disciplineId)).Returns(existingDiscipline);

            // Act
            _academicService.AssignGrade(gradeDto);

            // Assert
            _gradeRepositoryMock.Verify(r => r.Add(It.Is<Grade>(g =>
                g.DisciplineId == disciplineId &&
                g.StudentId == gradeDto.StudentId &&
                g.Value == gradeDto.Value)), Times.Once);

            _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }
    }
}