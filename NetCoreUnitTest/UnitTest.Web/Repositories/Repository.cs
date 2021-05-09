using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UnitTest.Web.Models;

namespace UnitTest.Web.Repositories
{
    public partial class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly UnitTestDBContext _context;
        private readonly DbSet<TEntity> _dbSet;
        
        public Repository(UnitTestDBContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
