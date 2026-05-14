using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.BLL.Services;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.Repositories;
using DigitalCollege.DAL.UnitOfWork;
using Moq;
using Xunit;

namespace DigitalCollege.Tests.ServicesTests
{
    public class ManagementServiceTests
    {
        [Fact]
        public void AddTeacher_EmptyName_ThrowsBusinessLogicException()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var service = new ManagementService(unitOfWorkMock.Object);
            var teacherDto = new TeacherDto { FullName = "" };

            Assert.Throws<BusinessLogicException>(() => service.AddTeacher(teacherDto));
        }

        [Fact]
        public void AddTeacher_ValidData_CallsAddAndSave()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var teacherRepoMock = new Mock<IRepository<Teacher>>();
            
            unitOfWorkMock.Setup(uow => uow.Teachers).Returns(teacherRepoMock.Object);
            var service = new ManagementService(unitOfWorkMock.Object);
            var teacherDto = new TeacherDto { FullName = "Іванов І.І." };

            service.AddTeacher(teacherDto);

            teacherRepoMock.Verify(r => r.Add(It.IsAny<Teacher>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }
    }
}