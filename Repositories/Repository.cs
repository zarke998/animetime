using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Repositories.Interfaces;
using System.Data.Entity;
using System.Data.SqlClient;

namespace AnimeTime.Persistence.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        private string _repoName;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            var properties = _dbContext.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DbSet<TEntity>))
                {
                    _repoName = property.Name;
                    break;
                }
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

        public void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public int GetLastInsertId()
        {
            if (_repoName == null)
                throw new ArgumentNullException("Could not get remote repository name.");

            return _dbContext.Database.SqlQuery<int>("SELECT dbo.LastInsertId(@table)", new SqlParameter("@table", _repoName)).Single();
        }
    }
}
