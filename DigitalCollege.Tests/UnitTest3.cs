using Moq;
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
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDisciplineRepository = new Mock<IRepository<Discipline>>();
            _mockGradeRepository = new Mock<IRepository<Grade>>();


            _mockUnitOfWork.Setup(u => u.Disciplines).Returns(_mockDisciplineRepository.Object);
            _mockUnitOfWork.Setup(u => u.Grades).Returns(_mockGradeRepository.Object);


            _academicService = new AcademicService(_mockUnitOfWork.Object);
        }

        [Fact]
        public void AssignGrade_ShouldThrowBusinessLogicException_WhenTeacherAssignsGradeForSomeoneElsesDiscipline()
        {

            int realTeacherId = 10;
            int hackerTeacherId = 99;
            int disciplineId = 1;


            var fakeDiscipline = new Discipline
            {
                Id = disciplineId,
                Name = "Вища математика",
                TeacherId = realTeacherId
            };
            _mockDisciplineRepository.Setup(r => r.GetById(disciplineId)).Returns(fakeDiscipline);


            var gradeDto = new GradeDto
            {
                StudentId = 5,
                DisciplineId = disciplineId,
                TeacherId = hackerTeacherId,
                Value = 95
            };


            var exception = Assert.Throws<BusinessLogicException>(() => _academicService.AssignGrade(gradeDto));


            Assert.Contains("Ви не можете виставляти оцінки по чужій дисципліні.", exception.Message);


            _mockGradeRepository.Verify(r => r.Add(It.IsAny<Grade>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        }
    }
}
