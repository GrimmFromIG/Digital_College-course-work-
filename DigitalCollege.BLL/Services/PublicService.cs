using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.DAL.UnitOfWork;

namespace DigitalCollege.BLL.Services
{
    public class PublicService : IPublicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TeacherDto> GetAllTeachersInfo()
        {
            return _unitOfWork.Teachers.GetAll().Select(t => new TeacherDto
            {
                Id = t.Id,
                FullName = t.FullName
            });
        }

        public IEnumerable<DisciplineDto> GetAllDisciplinesInfo()
        {
            return _unitOfWork.Disciplines.GetAll().Select(d => new DisciplineDto
            {
                Id = d.Id,
                Name = d.Name,
                TeacherId = d.TeacherId
            });
        }
    }
}
