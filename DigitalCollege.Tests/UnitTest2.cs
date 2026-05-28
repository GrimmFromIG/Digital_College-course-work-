using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DigitalCollege.BLL.Services;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.DAL.UnitOfWork;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.Repositories;

namespace DigitalCollege.Tests
{
    public class ManagementServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRepository<Group>> _mockGroupRepository;
        private readonly Mock<IRepository<Student>> _mockStudentRepository;
        private readonly ManagementService _managementService;

        public ManagementServiceUnitTests()
        {
            // Инициализируем моки для UnitOfWork и репозиториев
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockGroupRepository = new Mock<IRepository<Group>>();
            _mockStudentRepository = new Mock<IRepository<Student>>();

            // Настраиваем UnitOfWork, чтобы он возвращал наши моки репозиториев
            _mockUnitOfWork.Setup(u => u.Groups).Returns(_mockGroupRepository.Object);
            _mockUnitOfWork.Setup(u => u.Students).Returns(_mockStudentRepository.Object);

            // Создаем экземпляр тестируемого сервиса, внедряя замоканный UnitOfWork
            _managementService = new ManagementService(_mockUnitOfWork.Object);
        }

        [Fact]
        public void AddGroup_ShouldAddGroupAndCallSave_WhenCalled()
        {
            // Arrange (Подготовка)
            var groupDto = new GroupDto { Name = "КН-21" };

            // Act (Действие)
            _managementService.AddGroup(groupDto);

            // Assert (Проверка результатов)
            // Проверяем, что метод Add репозитория групп был вызван ровно один раз с правильными параметрами
            _mockGroupRepository.Verify(r => r.Add(It.Is<Group>(g => g.Name == groupDto.Name)), Times.Once);

            // Проверяем, что изменения были сохранены в базу данных
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void DeleteGroup_ShouldThrowBusinessLogicException_WhenGroupHasStudents()
        {
            // Arrange (Подготовка)
            int groupId = 1;

            // Создаем тестовый список студентов, привязанных к этой группе
            var studentsInGroup = new List<Student>
            {
                new Student { Id = 1, FullName = "Иван Иванов", GroupId = groupId }
            };

            // Настраиваем метод Find репозитория студентов.
            // Он должен перехватывать лямбда-выражение и выполнять его на нашем тестовом списке
            _mockStudentRepository
                .Setup(r => r.Find(It.IsAny<Expression<Func<Student, bool>>>()))
                .Returns((Expression<Func<Student, bool>> predicate) =>
                    studentsInGroup.Where(predicate.Compile()).ToList());

            // Act & Assert (Действие и Проверка)
            // Проверяем, что вызывается ошибка бизнес-логики
            var exception = Assert.Throws<BusinessLogicException>(() => _managementService.DeleteGroup(groupId));

            // Проверяем текст сообщения об ошибке
            Assert.Contains("Неможливо видалити групу: до неї прив'язані студенти", exception.Message);

            // Гарантируем, что методы удаления группы и сохранения изменений НЕ были вызваны
            _mockGroupRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public void DeleteGroup_ShouldDeleteAndSave_WhenGroupIsEmpty()
        {
            // Arrange (Подготовка)
            int groupId = 2;
            var emptyStudentsList = new List<Student>();

            // Метод Find возвращает пустой список (студентов в группе нет)
            _mockStudentRepository
                .Setup(r => r.Find(It.IsAny<Expression<Func<Student, bool>>>()))
                .Returns((Expression<Func<Student, bool>> predicate) =>
                    emptyStudentsList.Where(predicate.Compile()).ToList());

            // Act (Действие)
            _managementService.DeleteGroup(groupId);

            // Assert (Проверка результатов)
            // Проверяем, что метод Delete был вызван именно для этого ID группы
            _mockGroupRepository.Verify(r => r.Delete(groupId), Times.Once);

            // Проверяем фиксацию транзакции
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }
    }
}