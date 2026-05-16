using DigitalCollege.BLL.DTOs;

namespace DigitalCollege.BLL.Interfaces
{
    public interface IManagementService
    {
        void AddTeacher(TeacherDto teacherDto);
        void UpdateTeacher(TeacherDto teacherDto);
        void DeleteTeacher(int id);
        IEnumerable<TeacherDto> GetTeachers(string searchTerm = null, int? disciplineId = null, string sortBy = null);

        void AddStudent(StudentDto studentDto);
        void UpdateStudent(StudentDto studentDto);
        void DeleteStudent(int id);

        IEnumerable<StudentDto> GetStudents(string searchTerm = null, string groupName = null, string sortBy = null);

        void AddDiscipline(DisciplineDto dto);
        void UpdateDiscipline(DisciplineDto dto);
        void DeleteDiscipline(int id);
        IEnumerable<DisciplineDto> GetDisciplines(string searchTerm = null, string sortBy = null, int? teacherId = null);

        void AddGroup(GroupDto groupDto);
        void UpdateGroup(GroupDto groupDto);
        void DeleteGroup(int id);
        IEnumerable<GroupDto> GetGroups(string searchTerm = null);
    }
}
