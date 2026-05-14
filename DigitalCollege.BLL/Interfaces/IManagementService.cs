using DigitalCollege.BLL.DTOs;
using System.Collections.Generic;

namespace DigitalCollege.BLL.Interfaces
{
    public interface IManagementService
    {
        void AddTeacher(TeacherDto teacherDto);
        void UpdateTeacher(TeacherDto teacherDto);
        void DeleteTeacher(int id);
        IEnumerable<TeacherDto> GetTeachers(string searchTerm = null, string sortBy = null);
        
        void AddStudent(StudentDto studentDto);
        void UpdateStudent(StudentDto studentDto);
        void DeleteStudent(int id);
        IEnumerable<StudentDto> GetStudents(string searchTerm = null, int? groupId = null, string sortBy = null);
        
        void AddDiscipline(DisciplineDto disciplineDto);
        void UpdateDiscipline(DisciplineDto disciplineDto);
        void DeleteDiscipline(int id);
        IEnumerable<DisciplineDto> GetDisciplines(string searchTerm = null, string sortBy = null);
    }
}