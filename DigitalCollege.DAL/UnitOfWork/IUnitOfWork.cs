using DigitalCollege.DAL.Entities;

namespace DigitalCollege.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Repositories.IRepository<Teacher> Teachers { get; }
        Repositories.IRepository<Student> Students { get; }
        Repositories.IRepository<Group> Groups { get; }
        Repositories.IRepository<Discipline> Disciplines { get; }
        Repositories.IRepository<Grade> Grades { get; }
        Repositories.IRepository<User> Users { get; }
        
        void Save();
    }
}
