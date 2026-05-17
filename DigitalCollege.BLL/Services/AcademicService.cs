using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.UnitOfWork;

namespace DigitalCollege.BLL.Services
{
    public class AcademicService : IAcademicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AcademicService(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        public void AssignGrade(GradeDto gradeDto)
        {
            var discipline = _unitOfWork.Disciplines.GetById(gradeDto.DisciplineId);
            if (discipline == null) throw new BusinessLogicException("Дисципліну не знайдено.");
            if (discipline.TeacherId != gradeDto.TeacherId) throw new BusinessLogicException("Ви не можете виставляти оцінки по чужій дисципліні.");

            var grade = new Grade
            {
                Value = gradeDto.Value,
                DateAssigned = System.DateTime.Now,
                StudentId = gradeDto.StudentId,
                DisciplineId = gradeDto.DisciplineId
            };
            _unitOfWork.Grades.Add(grade);
            _unitOfWork.Save();
        }

        public void UpdateGrade(GradeDto gradeDto)
        {
            var grade = _unitOfWork.Grades.GetById(gradeDto.Id);
            if (grade == null) throw new BusinessLogicException("Оцінку не знайдено.");

            var discipline = _unitOfWork.Disciplines.GetById(grade.DisciplineId);
            if (discipline.TeacherId != gradeDto.TeacherId) throw new BusinessLogicException("Ви не можете змінювати оцінку по чужій дисципліні.");

            grade.Value = gradeDto.Value;
            _unitOfWork.Grades.Update(grade);
            _unitOfWork.Save();
        }

        public void DeleteGrade(int gradeId, int teacherId)
        {
            var grade = _unitOfWork.Grades.GetById(gradeId);
            if (grade == null) throw new BusinessLogicException("Оцінку не знайдено.");

            var discipline = _unitOfWork.Disciplines.GetById(grade.DisciplineId);
            if (discipline.TeacherId != teacherId) throw new BusinessLogicException("Ви не можете видаляти оцінку по чужій дисципліні.");

            _unitOfWork.Grades.Delete(gradeId);
            _unitOfWork.Save();
        }

        public IEnumerable<GradeDto> GetStudentGrades(int studentId)
        {
            var disciplines = _unitOfWork.Disciplines.GetAll().ToList();
            var teachers = _unitOfWork.Teachers.GetAll().ToList();

            return _unitOfWork.Grades.Find(g => g.StudentId == studentId).Select(g =>
            {
                var disc = disciplines.FirstOrDefault(d => d.Id == g.DisciplineId);
                return new GradeDto
                {
                    Id = g.Id,
                    Value = g.Value,
                    DateAssigned = g.DateAssigned,
                    StudentName = g.Student?.FullName,
                    DisciplineName = disc?.Name,
                    DisciplineId = g.DisciplineId,
                    TeacherName = disc != null ? teachers.FirstOrDefault(t => t.Id == disc.TeacherId)?.FullName : null
                };
            }).ToList();
        }

        public IEnumerable<GradeDto> GetDisciplineGrades(int disciplineId)
        {
            return _unitOfWork.Grades.Find(g => g.DisciplineId == disciplineId)
                .Select(g => new GradeDto { Id = g.Id, Value = g.Value, DateAssigned = g.DateAssigned, StudentName = g.Student?.FullName, DisciplineName = g.Discipline?.Name, DisciplineId = g.DisciplineId }).ToList();
        }
    }
}
