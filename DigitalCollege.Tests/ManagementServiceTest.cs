using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.BLL.Services;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.Repositories;
using DigitalCollege.DAL.UnitOfWork;

namespace DigitalCollege.BLL.Tests
{
    public class ManagementServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Group>> _groupRepositoryMock;
        private readonly Mock<IRepository<Student>> _studentRepositoryMock;
        private readonly Mock<IRepository<Teacher>> _teacherRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly ManagementService _managementService;

        public ManagementServiceTests()
        {
            // Ініціалізація моків (замінювачів реальних даних)
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _groupRepositoryMock = new Mock<IRepository<Group>>();
            _studentRepositoryMock = new Mock<IRepository<Student>>();
            _teacherRepositoryMock = new Mock<IRepository<Teacher>>();
            _userRepositoryMock = new Mock<IRepository<User>>();

            // Налаштування UnitOfWork для повернення макетів репозиторіїв
            _unitOfWorkMock.Setup(u => u.Groups).Returns(_groupRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Students).Returns(_studentRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Teachers).Returns(_teacherRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);

            // Впровадження залежностей (DI) через конструктор
            _managementService = new ManagementService(_unitOfWorkMock.Object);
        }

        [Fact]
        public void AddTeacher_ShouldThrowException_WhenFullNameIsEmpty()
        {
            // Arrange (Встановлення умов)
            var teacherDto = new TeacherDto
            {
                FullName = "",
                Email = "test@college.ua",
                Password = "password123"
            };

            // Act & Assert (Дія та Перевірка результату)
            var exception = Assert.Throws<BusinessLogicException>(() => _managementService.AddTeacher(teacherDto));
            Assert.Equal("ПІБ викладача не може бути порожнім.", exception.Message);
        }

        [Fact]
        public void AddTeacher_ShouldThrowException_WhenEmailInvalidFormat()
        {
            // Arrange
            var teacherDto = new TeacherDto
            {
                FullName = "Іван Іванов",
                Email = "ivan@gmail.com", // Некоректний домен
                Password = "password123"
            };

            // Act & Assert
            var exception = Assert.Throws<BusinessLogicException>(() => _managementService.AddTeacher(teacherDto));
            Assert.Equal("Електронна пошта обов'язково повинна мати формат email@college.ua", exception.Message);
        }

        [Fact]
        public void AddGroup_ShouldSaveGroup_WhenDataIsValid()
        {
            // Arrange
            var groupDto = new GroupDto { Name = "КН-21" };

            // Act
            _managementService.AddGroup(groupDto);

            // Assert
            _groupRepositoryMock.Verify(r => r.Add(It.Is<Group>(g => g.Name == groupDto.Name)), Times.Once);
            _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void DeleteGroup_ShouldThrowException_WhenStudentsAreAssignedToGroup()
        {
            // Arrange
            int groupId = 1;
            var assignedStudents = new List<Student> { new Student { Id = 1, GroupId = groupId } };

            _studentRepositoryMock
                .Setup(r => r.Find(It.IsAny<Expression<Func<Student, bool>>>()))
                .Returns(assignedStudents);

            // Act & Assert
            var exception = Assert.Throws<BusinessLogicException>(() => _managementService.DeleteGroup(groupId));
            Assert.Contains("Неможливо видалити групу: до неї прив'язані студенти", exception.Message);

            // Перевірка, що метод видалення НЕ був викликаний
            _groupRepositoryMock.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteGroup_ShouldDeleteSuccessfully_WhenGroupIsEmpty()
        {
            // Arrange
            int groupId = 2;
            _studentRepositoryMock
                .Setup(r => r.Find(It.IsAny<Expression<Func<Student, bool>>>()))
                .Returns(new List<Student>()); // Студентів немає

            // Act
            _managementService.DeleteGroup(groupId);

            // Assert
            _groupRepositoryMock.Verify(r => r.Delete(groupId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }
    }
}