using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Repositories;
using System.Data.Entity;

namespace AnimeTime.Persistence.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Attach(TEntity entity)
        {
            _dbContext.Set<TEntity>().Attach(entity);
        }
        public void AttachRange(IEnumerable<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                _dbContext.Set<TEntity>().Attach(entity);
            }
        }

        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Where(expression).ToList();
        }
        public TEntity Get(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }
        public IEnumerable<TEntity> GetAllCached()
        {
            var entries = _dbContext.ChangeTracker.Entries<TEntity>();
            var entities = entries.Select(e => e.Entity);

            return entities;
        }

        public void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }
    }
}
