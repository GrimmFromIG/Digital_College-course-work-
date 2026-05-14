using DigitalCollege.BLL.DTOs;
using System.Collections.Generic;

namespace DigitalCollege.BLL.Interfaces
{
    public interface IAcademicService
    {
        void AssignGrade(GradeDto gradeDto);
        IEnumerable<GradeDto> GetStudentGrades(int studentId);
        IEnumerable<GradeDto> GetDisciplineGrades(int disciplineId);
    }
}