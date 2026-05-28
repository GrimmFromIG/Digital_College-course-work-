using Moq;
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
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockGroupRepository = new Mock<IRepository<Group>>();
            _mockStudentRepository = new Mock<IRepository<Student>>();

            _mockUnitOfWork.Setup(u => u.Groups).Returns(_mockGroupRepository.Object);
            _mockUnitOfWork.Setup(u => u.Students).Returns(_mockStudentRepository.Object);

            _managementService = new ManagementService(_mockUnitOfWork.Object);
        }

        [Fact]
        public void AddGroup_ShouldAddGroupAndCallSave_WhenCalled()
        {

            var groupDto = new GroupDto { Name = "КН-21" };

            _managementService.AddGroup(groupDto);


            _mockGroupRepository.Verify(r => r.Add(It.Is<Group>(g => g.Name == groupDto.Name)), Times.Once);


            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void DeleteGroup_ShouldThrowBusinessLogicException_WhenGroupHasStudents()
        {

            int groupId = 1;


            var studentsInGroup = new List<Student>
            {
                new Student { Id = 1, FullName = "Иван Иванов", GroupId = groupId }
            };


            _mockStudentRepository
                .Setup(r => r.Find(It.IsAny<Expression<Func<Student, bool>>>()))
                .Returns((Expression<Func<Student, bool>> predicate) =>
                    studentsInGroup.Where(predicate.Compile()).ToList());


            var exception = Assert.Throws<BusinessLogicException>(() => _managementService.DeleteGroup(groupId));


            Assert.Contains("Неможливо видалити групу: до неї прив'язані студенти", exception.Message);

 
            _mockGroupRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public void DeleteGroup_ShouldDeleteAndSave_WhenGroupIsEmpty()
        {

            int groupId = 2;
            var emptyStudentsList = new List<Student>();


            _mockStudentRepository
                .Setup(r => r.Find(It.IsAny<Expression<Func<Student, bool>>>()))
                .Returns((Expression<Func<Student, bool>> predicate) =>
                    emptyStudentsList.Where(predicate.Compile()).ToList());


            _managementService.DeleteGroup(groupId);


            _mockGroupRepository.Verify(r => r.Delete(groupId), Times.Once);


            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }
    }
}