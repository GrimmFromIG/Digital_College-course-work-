using System;
using Moq;
using Xunit;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.BLL.Services;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.UnitOfWork;
using DigitalCollege.DAL.Repositories;

namespace DigitalCollege.Tests
{
    public class AcademicServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRepository<Discipline>> _mockDisciplineRepository;
        private readonly Mock<IRepository<Grade>> _mockGradeRepository;
        private readonly AcademicService _academicService;

        public AcademicServiceTests()
        {
            // Инициализируем моки для UnitOfWork и репозиториев
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDisciplineRepository = new Mock<IRepository<Discipline>>();
            _mockGradeRepository = new Mock<IRepository<Grade>>();

            // Настраиваем связь UnitOfWork с репозиториями[cite: 35]
            _mockUnitOfWork.Setup(u => u.Disciplines).Returns(_mockDisciplineRepository.Object);
            _mockUnitOfWork.Setup(u => u.Grades).Returns(_mockGradeRepository.Object);

            // Создаем экземпляр тестируемого сервиса оценок
            _academicService = new AcademicService(_mockUnitOfWork.Object);
        }

        [Fact]
        public void AssignGrade_ShouldThrowBusinessLogicException_WhenTeacherAssignsGradeForSomeoneElsesDiscipline()
        {
            // Arrange (Подготовка данных)
            int realTeacherId = 10;
            int hackerTeacherId = 99; // Этот преподаватель пытается поставить оценку
            int disciplineId = 1;

            // Настраиваем мок так, будто в базе данных есть дисциплина, принадлежащая realTeacherId
            var fakeDiscipline = new Discipline
            {
                Id = disciplineId,
                Name = "Вища математика",
                TeacherId = realTeacherId
            };
            _mockDisciplineRepository.Setup(r => r.GetById(disciplineId)).Returns(fakeDiscipline);

            // Формируем запрос на создание оценки от имени "чужого" преподавателя
            var gradeDto = new GradeDto
            {
                StudentId = 5,
                DisciplineId = disciplineId,
                TeacherId = hackerTeacherId, // Передаем ID другого преподавателя
                Value = 95
            };

            // Act & Assert (Действие и Проверка исключения)
            // Метод должен выбросить ошибку бизнес-логики
            var exception = Assert.Throws<BusinessLogicException>(() => _academicService.AssignGrade(gradeDto));

            // Проверяем, что текст ошибки совпадает с заложенным в сервисе
            Assert.Contains("Ви не можете виставляти оцінки по чужій дисципліні.", exception.Message);

            // Гарантируем, что оценка НЕ была добавлена в репозиторий и метод Save() НЕ вызывался
            _mockGradeRepository.Verify(r => r.Add(It.IsAny<Grade>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        }
    }
}
