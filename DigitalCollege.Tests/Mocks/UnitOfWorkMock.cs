using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.Repositories;
using DigitalCollege.DAL.UnitOfWork;
using Moq;

namespace DigitalCollege.Tests.Mocks
{
    public static class UnitOfWorkMock
    {
        public static Mock<IUnitOfWork> GetMock()
        {
            var mock = new Mock<IUnitOfWork>();
            
            mock.Setup(m => m.Teachers).Returns(new Mock<IRepository<Teacher>>().Object);
            mock.Setup(m => m.Students).Returns(new Mock<IRepository<Student>>().Object);
            mock.Setup(m => m.Groups).Returns(new Mock<IRepository<Group>>().Object);
            mock.Setup(m => m.Disciplines).Returns(new Mock<IRepository<Discipline>>().Object);
            mock.Setup(m => m.Grades).Returns(new Mock<IRepository<Grade>>().Object);
            
            return mock;
        }
    }
}