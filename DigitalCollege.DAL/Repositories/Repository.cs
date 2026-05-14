using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DigitalCollege.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T GetById(int id) => _dbSet.Find(id);

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public void Add(T entity) => _dbSet.Add(entity);

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            T entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}