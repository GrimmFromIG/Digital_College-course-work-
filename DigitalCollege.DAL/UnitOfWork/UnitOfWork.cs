using System;
using DigitalCollege.DAL.Context;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.Repositories;

namespace DigitalCollege.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DepartmentDbContext _context;
        
        private IRepository<Teacher> _teacherRepository;
        private IRepository<Student> _studentRepository;
        private IRepository<Group> _groupRepository;
        private IRepository<Discipline> _disciplineRepository;
        private IRepository<Grade> _gradeRepository;

        public UnitOfWork(DepartmentDbContext context)
        {
            _context = context;
        }

        public IRepository<Teacher> Teachers => _teacherRepository ??= new Repository<Teacher>(_context);
        public IRepository<Student> Students => _studentRepository ??= new Repository<Student>(_context);
        public IRepository<Group> Groups => _groupRepository ??= new Repository<Group>(_context);
        public IRepository<Discipline> Disciplines => _disciplineRepository ??= new Repository<Discipline>(_context);
        public IRepository<Grade> Grades => _gradeRepository ??= new Repository<Grade>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}