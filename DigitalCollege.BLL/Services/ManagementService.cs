using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.UnitOfWork;

namespace DigitalCollege.BLL.Services
{
    public class ManagementService : IManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private void EnsureValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;
            if (!email.EndsWith("@college.ua", System.StringComparison.OrdinalIgnoreCase))
                throw new BusinessLogicException("Електронна пошта обов'язково повинна мати формат email@college.ua");
        }

        public void AddTeacher(TeacherDto teacherDto)
        {
            teacherDto.Email = teacherDto.Email?.Replace(" ", "");
            teacherDto.Password = teacherDto.Password?.Replace(" ", "");

            if (string.IsNullOrWhiteSpace(teacherDto.FullName)) throw new BusinessLogicException("ПІБ викладача не може бути порожнім.");
            if (string.IsNullOrWhiteSpace(teacherDto.Email) || string.IsNullOrWhiteSpace(teacherDto.Password)) throw new BusinessLogicException("Email та пароль обов'язкові.");

            EnsureValidEmail(teacherDto.Email);

            if (_unitOfWork.Users.Find(u => u.Email == teacherDto.Email).Any()) throw new BusinessLogicException("Користувач з таким Email вже існує.");

            var teacher = new Teacher { FullName = teacherDto.FullName };
            _unitOfWork.Teachers.Add(teacher);
            _unitOfWork.Save();

            var user = new User { Email = teacherDto.Email, PasswordHash = teacherDto.Password, Role = "Teacher", TeacherId = teacher.Id };
            _unitOfWork.Users.Add(user);
            _unitOfWork.Save();
        }

        public void UpdateTeacher(TeacherDto teacherDto)
        {
            teacherDto.Email = teacherDto.Email?.Replace(" ", "");
            teacherDto.Password = teacherDto.Password?.Replace(" ", "");

            var teacher = _unitOfWork.Teachers.GetById(teacherDto.Id);
            if (teacher == null) throw new BusinessLogicException("Викладача не знайдено.");

            teacher.FullName = teacherDto.FullName;
            _unitOfWork.Teachers.Update(teacher);

            var user = _unitOfWork.Users.Find(u => u.TeacherId == teacher.Id).FirstOrDefault();
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(teacherDto.Email))
                {
                    EnsureValidEmail(teacherDto.Email);
                    if (user.Email != teacherDto.Email && _unitOfWork.Users.Find(u => u.Email == teacherDto.Email).Any())
                        throw new BusinessLogicException("Цей Email вже зайнятий.");
                    user.Email = teacherDto.Email;
                }
                if (!string.IsNullOrWhiteSpace(teacherDto.Password)) user.PasswordHash = teacherDto.Password;
                _unitOfWork.Users.Update(user);
            }
            _unitOfWork.Save();
        }

        public void DeleteTeacher(int id)
        {
            if (_unitOfWork.Disciplines.Find(d => d.TeacherId == id).Any())
                throw new BusinessLogicException("Неможливо видалити викладача: за ним закріплені дисципліни.");

            var teacher = _unitOfWork.Teachers.GetById(id);
            if (teacher == null) throw new BusinessLogicException("Викладача не знайдено.");
            var user = _unitOfWork.Users.Find(u => u.TeacherId == id).FirstOrDefault();
            if (user != null) _unitOfWork.Users.Delete(user.Id);
            _unitOfWork.Teachers.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<TeacherDto> GetTeachers(string searchTerm = null, int? disciplineId = null, string sortBy = null)
        {
            var teachers = _unitOfWork.Teachers.GetAll().ToList();
            var allUsers = _unitOfWork.Users.GetAll().Where(u => u.TeacherId != null).ToList();
            var allDisciplines = _unitOfWork.Disciplines.GetAll().ToList();

            if (disciplineId.HasValue && disciplineId.Value > 0)
            {
                var teacherIdsWithDiscipline = allDisciplines
                    .Where(d => d.Id == disciplineId.Value)
                    .Select(d => d.TeacherId)
                    .Distinct()
                    .ToList();
                teachers = teachers.Where(t => teacherIdsWithDiscipline.Contains(t.Id)).ToList();
            }

            var result = teachers.Select(t =>
            {
                var user = allUsers.FirstOrDefault(u => u.TeacherId == t.Id);
                var teacherDisciplines = allDisciplines.Where(d => d.TeacherId == t.Id).Select(d => d.Name).OrderBy(n => n).ToList();
                var disciplinesString = teacherDisciplines.Any() ? string.Join(", ", teacherDisciplines) : "Без дисциплін";

                return new TeacherDto
                {
                    Id = t.Id,
                    FullName = t.FullName,
                    Email = user != null ? user.Email : null,
                    Disciplines = disciplinesString
                };
            });

            if (!string.IsNullOrWhiteSpace(searchTerm))
                result = result.Where(t => t.FullName.ToLower().Contains(searchTerm.ToLower()));

            if (sortBy == "name_desc")
                result = result.OrderByDescending(t => t.FullName);
            else
                result = result.OrderBy(t => t.FullName);

            return result.ToList();
        }

        public void AddStudent(StudentDto studentDto)
        {
            studentDto.Email = studentDto.Email?.Replace(" ", "");
            studentDto.Password = studentDto.Password?.Replace(" ", "");

            if (string.IsNullOrWhiteSpace(studentDto.FullName)) throw new BusinessLogicException("Ім'я студента обов'язкове.");
            if (string.IsNullOrWhiteSpace(studentDto.Email) || string.IsNullOrWhiteSpace(studentDto.Password)) throw new BusinessLogicException("Email та пароль обов'язкові.");

            EnsureValidEmail(studentDto.Email);

            if (_unitOfWork.Users.Find(u => u.Email == studentDto.Email).Any()) throw new BusinessLogicException("Користувач з таким Email вже існує.");

            var student = new Student { FullName = studentDto.FullName, GroupId = studentDto.GroupId };
            _unitOfWork.Students.Add(student);
            _unitOfWork.Save();

            var user = new User { Email = studentDto.Email, PasswordHash = studentDto.Password, Role = "Student", StudentId = student.Id };
            _unitOfWork.Users.Add(user);
            _unitOfWork.Save();
        }

        public void UpdateStudent(StudentDto studentDto)
        {
            studentDto.Email = studentDto.Email?.Replace(" ", "");
            studentDto.Password = studentDto.Password?.Replace(" ", "");

            var student = _unitOfWork.Students.GetById(studentDto.Id);
            if (student == null) throw new BusinessLogicException("Студента не знайдено.");

            student.FullName = studentDto.FullName;
            student.GroupId = studentDto.GroupId;
            _unitOfWork.Students.Update(student);

            var user = _unitOfWork.Users.Find(u => u.StudentId == student.Id).FirstOrDefault();
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(studentDto.Email))
                {
                    EnsureValidEmail(studentDto.Email);
                    if (user.Email != studentDto.Email && _unitOfWork.Users.Find(u => u.Email == studentDto.Email).Any())
                        throw new BusinessLogicException("Цей Email вже зайнятий.");
                    user.Email = studentDto.Email;
                }
                if (!string.IsNullOrWhiteSpace(studentDto.Password)) user.PasswordHash = studentDto.Password;
                _unitOfWork.Users.Update(user);
            }
            _unitOfWork.Save();
        }

        public void DeleteStudent(int id)
        {
            if (_unitOfWork.Grades.Find(g => g.StudentId == id).Any())
                throw new BusinessLogicException("Неможливо видалити студента: у нього є виставлені оцінки в журналі.");

            var student = _unitOfWork.Students.GetById(id);
            if (student == null) throw new BusinessLogicException("Студента не знайдено.");
            var user = _unitOfWork.Users.Find(u => u.StudentId == id).FirstOrDefault();
            if (user != null) _unitOfWork.Users.Delete(user.Id);
            _unitOfWork.Students.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<StudentDto> GetStudents(string searchTerm = null, string groupName = null, string sortBy = null)
        {
            var allGroups = _unitOfWork.Groups.GetAll().ToList();
            var allUsers = _unitOfWork.Users.GetAll().Where(u => u.StudentId != null).ToList();
            var students = _unitOfWork.Students.GetAll().ToList();

            var result = students.Select(s => {
                var group = allGroups.FirstOrDefault(g => g.Id == s.GroupId);
                var user = allUsers.FirstOrDefault(u => u.StudentId == s.Id);
                return new StudentDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    GroupId = s.GroupId,
                    GroupName = group != null ? group.Name : "Без групи",
                    Email = user != null ? user.Email : null
                };
            });

            if (!string.IsNullOrWhiteSpace(searchTerm))
                result = result.Where(s => s.FullName.ToLower().Contains(searchTerm.ToLower()));

            if (!string.IsNullOrWhiteSpace(groupName))
                result = result.Where(s => s.GroupName.ToLower().Contains(groupName.ToLower()));

            if (sortBy == "name_desc")
                result = result.OrderByDescending(s => s.FullName);
            else
                result = result.OrderBy(s => s.FullName);

            return result.ToList();
        }

        public void AddDiscipline(DisciplineDto dto) { _unitOfWork.Disciplines.Add(new Discipline { Name = dto.Name, TeacherId = dto.TeacherId }); _unitOfWork.Save(); }
        public void UpdateDiscipline(DisciplineDto dto) { var d = _unitOfWork.Disciplines.GetById(dto.Id); if (d != null) { d.Name = dto.Name; d.TeacherId = dto.TeacherId; _unitOfWork.Disciplines.Update(d); _unitOfWork.Save(); } }
        public void DeleteDiscipline(int id)
        {
            if (_unitOfWork.Grades.Find(g => g.DisciplineId == id).Any())
                throw new BusinessLogicException("Неможливо видалити дисципліну: по ній вже виставлено оцінки студентам.");

            _unitOfWork.Disciplines.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<DisciplineDto> GetDisciplines(string searchTerm = null, string sortBy = null, int? teacherId = null)
        {
            var query = _unitOfWork.Disciplines.GetAll().AsEnumerable();
            var teachers = _unitOfWork.Teachers.GetAll().ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(d => d.Name.ToLower().Contains(searchTerm.ToLower()));

            if (teacherId.HasValue && teacherId.Value > 0)
                query = query.Where(d => d.TeacherId == teacherId.Value);

            if (sortBy == "name_desc") query = query.OrderByDescending(d => d.Name);
            else query = query.OrderBy(d => d.Name);

            return query.Select(d => new DisciplineDto
            {
                Id = d.Id,
                Name = d.Name,
                TeacherId = d.TeacherId,
                TeacherName = teachers.FirstOrDefault(t => t.Id == d.TeacherId)?.FullName
            }).ToList();
        }

        public void AddGroup(GroupDto dto) { _unitOfWork.Groups.Add(new Group { Name = dto.Name }); _unitOfWork.Save(); }
        public void UpdateGroup(GroupDto dto) { var g = _unitOfWork.Groups.GetById(dto.Id); if (g != null) { g.Name = dto.Name; _unitOfWork.Groups.Update(g); _unitOfWork.Save(); } }
        public void DeleteGroup(int id)
        {
            if (_unitOfWork.Students.Find(s => s.GroupId == id).Any())
                throw new BusinessLogicException("Неможливо видалити групу: до неї прив'язані студенти. Спочатку переведіть їх в іншу групу або видаліть.");

            _unitOfWork.Groups.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<GroupDto> GetGroups(string searchTerm = null)
        {
            var query = _unitOfWork.Groups.GetAll().AsEnumerable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(g => g.Name.ToLower().Contains(searchTerm.ToLower()));

            return query.OrderBy(g => g.Name).Select(g => new GroupDto { Id = g.Id, Name = g.Name }).ToList();
        }
    }
}
