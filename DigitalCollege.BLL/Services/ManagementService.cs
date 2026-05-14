using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace DigitalCollege.BLL.Services
{
    public class ManagementService : IManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddTeacher(TeacherDto teacherDto)
        {
            if (string.IsNullOrWhiteSpace(teacherDto.FullName))
                throw new BusinessLogicException("ПІБ викладача не може бути порожнім.");

            _unitOfWork.Teachers.Add(new Teacher { FullName = teacherDto.FullName });
            _unitOfWork.Save();
        }

        public void UpdateTeacher(TeacherDto teacherDto)
        {
            var teacher = _unitOfWork.Teachers.GetById(teacherDto.Id);
            if (teacher == null)
                throw new BusinessLogicException("Викладача не знайдено.");

            teacher.FullName = teacherDto.FullName;
            _unitOfWork.Teachers.Update(teacher);
            _unitOfWork.Save();
        }

        public void DeleteTeacher(int id)
        {
            var teacher = _unitOfWork.Teachers.GetById(id);
            if (teacher == null)
                throw new BusinessLogicException("Викладача не знайдено.");

            _unitOfWork.Teachers.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<TeacherDto> GetTeachers(string searchTerm = null, string sortBy = null)
        {
            var data = _unitOfWork.Teachers.Find(t => 
                string.IsNullOrWhiteSpace(searchTerm) || t.FullName.ToLower().Contains(searchTerm.ToLower()));

            var query = data.AsQueryable();

            if (sortBy == "name_desc")
                query = query.OrderByDescending(t => t.FullName);
            else
                query = query.OrderBy(t => t.FullName);

            return query.Select(t => new TeacherDto { Id = t.Id, FullName = t.FullName }).ToList();
        }

        public void AddStudent(StudentDto studentDto)
        {
            if (string.IsNullOrWhiteSpace(studentDto.FullName))
                throw new BusinessLogicException("Ім'я студента обов'язкове.");

            _unitOfWork.Students.Add(new Student { FullName = studentDto.FullName, GroupId = studentDto.GroupId });
            _unitOfWork.Save();
        }

        public void UpdateStudent(StudentDto studentDto)
        {
            var student = _unitOfWork.Students.GetById(studentDto.Id);
            if (student == null)
                throw new BusinessLogicException("Студента не знайдено.");

            student.FullName = studentDto.FullName;
            student.GroupId = studentDto.GroupId;
            _unitOfWork.Students.Update(student);
            _unitOfWork.Save();
        }

        public void DeleteStudent(int id)
        {
            var student = _unitOfWork.Students.GetById(id);
            if (student == null)
                throw new BusinessLogicException("Студента не знайдено.");

            _unitOfWork.Students.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<StudentDto> GetStudents(string searchTerm = null, int? groupId = null, string sortBy = null)
        {
            var data = _unitOfWork.Students.Find(s => 
                (string.IsNullOrWhiteSpace(searchTerm) || s.FullName.ToLower().Contains(searchTerm.ToLower())) &&
                (!groupId.HasValue || s.GroupId == groupId.Value));

            var query = data.AsQueryable();

            if (sortBy == "name_desc")
                query = query.OrderByDescending(s => s.FullName);
            else
                query = query.OrderBy(s => s.FullName);

            return query.Select(s => new StudentDto { Id = s.Id, FullName = s.FullName, GroupId = s.GroupId }).ToList();
        }

        public void AddDiscipline(DisciplineDto dto)
        {
            _unitOfWork.Disciplines.Add(new Discipline { Name = dto.Name, TeacherId = dto.TeacherId });
            _unitOfWork.Save();
        }

        public void UpdateDiscipline(DisciplineDto dto)
        {
            var discipline = _unitOfWork.Disciplines.GetById(dto.Id);
            if (discipline == null)
                throw new BusinessLogicException("Дисципліну не знайдено.");

            discipline.Name = dto.Name;
            discipline.TeacherId = dto.TeacherId;
            _unitOfWork.Disciplines.Update(discipline);
            _unitOfWork.Save();
        }

        public void DeleteDiscipline(int id)
        {
            _unitOfWork.Disciplines.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<DisciplineDto> GetDisciplines(string searchTerm = null, string sortBy = null)
        {
            var data = _unitOfWork.Disciplines.Find(d => 
                string.IsNullOrWhiteSpace(searchTerm) || d.Name.ToLower().Contains(searchTerm.ToLower()));

            var query = data.AsQueryable();

            if (sortBy == "name_desc")
                query = query.OrderByDescending(d => d.Name);
            else
                query = query.OrderBy(d => d.Name);

            return query.Select(d => new DisciplineDto { Id = d.Id, Name = d.Name, TeacherId = d.TeacherId }).ToList();
        }
    }
}