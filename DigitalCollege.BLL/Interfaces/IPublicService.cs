using DigitalCollege.BLL.DTOs;

namespace DigitalCollege.BLL.Interfaces
{
    public interface IPublicService
    {
        IEnumerable<TeacherDto> GetAllTeachersInfo();
        IEnumerable<DisciplineDto> GetAllDisciplinesInfo();
    }
}
