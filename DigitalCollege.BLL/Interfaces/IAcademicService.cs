using DigitalCollege.BLL.DTOs;
namespace DigitalCollege.BLL.Interfaces
{
    public interface IAcademicService
    {
        void AssignGrade(GradeDto gradeDto);
        void UpdateGrade(GradeDto gradeDto);
        void DeleteGrade(int gradeId, int teacherId);
        IEnumerable<GradeDto> GetStudentGrades(int studentId);
        IEnumerable<GradeDto> GetDisciplineGrades(int disciplineId);
    }
}
