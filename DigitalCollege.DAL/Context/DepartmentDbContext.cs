using DigitalCollege.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalCollege.DAL.Context
{
    public class DepartmentDbContext : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<User> Users { get; set; } 

        public DepartmentDbContext(DbContextOptions<DepartmentDbContext> options) : base(options)
        {
        }
    }
}