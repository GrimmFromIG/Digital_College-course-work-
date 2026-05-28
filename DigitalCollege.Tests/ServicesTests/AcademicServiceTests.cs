using Moq;
using DigitalCollege.BLL.Services;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.DAL.UnitOfWork;
using DigitalCollege.DAL.Entities;

namespace DigitalCollege.Tests.ServicesTests
{
    public class AcademicServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly AcademicService _service;

        public AcademicServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _service = new AcademicService(_uowMock.Object);
        }

        [Fact]
        public void AssignGrade_TeacherNotMatchingDiscipline_ThrowsException()
        {
            var gradeDto = new GradeDto 
            { 
                TeacherId = 1, 
                StudentId = 5, 
                DisciplineId = 10, 
                Value = 90 
            };

            var discipline = new Discipline 
            { 
                Id = 10, 
                TeacherId = 2,
                Name = "Test Discipline"
            };

            _uowMock.Setup(u => u.Disciplines.GetById(gradeDto.DisciplineId)).Returns(discipline);

            Assert.Throws<BusinessLogicException>(() => _service.AssignGrade(gradeDto));
        }

        [Fact]
        public void AssignGrade_ValidData_AddsGrade()
        {
            var gradeDto = new GradeDto 
            { 
                TeacherId = 1, 
                StudentId = 5, 
                DisciplineId = 10, 
                Value = 95 
            };

            var discipline = new Discipline 
            { 
                Id = 10, 
                TeacherId = 1,
                Name = "Test Discipline"
            };

            _uowMock.Setup(u => u.Disciplines.GetById(gradeDto.DisciplineId)).Returns(discipline);
            _uowMock.Setup(u => u.Grades.Add(It.IsAny<Grade>()));

            _service.AssignGrade(gradeDto);

            _uowMock.Verify(u => u.Grades.Add(It.IsAny<Grade>()), Times.Once);
            _uowMock.Verify(u => u.Save(), Times.Once);
        }
    }
}
