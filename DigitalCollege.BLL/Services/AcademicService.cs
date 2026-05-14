using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace DigitalCollege.BLL.Services
{
    public class AcademicService : IAcademicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AcademicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AssignGrade(GradeDto gradeDto)
        {
            var discipline = _unitOfWork.Disciplines.GetById(gradeDto.DisciplineId);
            
            if (discipline == null)
                throw new BusinessLogicException("Дисциплина не найдена.");

            if (discipline.TeacherId != gradeDto.TeacherId)
                throw new BusinessLogicException("Вы не можете выставлять оценки по чужой дисциплине.");

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

        public IEnumerable<GradeDto> GetStudentGrades(int studentId)
        {
            return _unitOfWork.Grades.Find(g => g.StudentId == studentId)
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    Value = g.Value,
                    DateAssigned = g.DateAssigned,
                    StudentName = g.Student?.FullName,
                    DisciplineName = g.Discipline?.Name,
                    DisciplineId = g.DisciplineId
                }).ToList();
        }

        public IEnumerable<GradeDto> GetDisciplineGrades(int disciplineId)
        {
            return _unitOfWork.Grades.Find(g => g.DisciplineId == disciplineId)
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    Value = g.Value,
                    DateAssigned = g.DateAssigned,
                    StudentName = g.Student?.FullName,
                    DisciplineName = g.Discipline?.Name,
                    DisciplineId = g.DisciplineId
                }).ToList();
        }
    }
}