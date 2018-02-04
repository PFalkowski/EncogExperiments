using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using Dapper;

namespace StocksData.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        public DbContext Context { get; }

        public IDbSet<TEntity> EntitiesDbSet => Context.Set<TEntity>();

        public IEnumerable<TEntity> Entities => EntitiesDbSet;

        public EfRepository(DbContext context)
        {
            this.Context = context;
        }

        public TEntity Get(object id)
        {
            return EntitiesDbSet.Find(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return EntitiesDbSet.FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return EntitiesDbSet.Where(predicate);
        }

        public IList<TEntity> GetAll()
        {
            return EntitiesDbSet.ToList();
        }

        public void Add(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            EntitiesDbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            ((DbSet<TEntity>)EntitiesDbSet).AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            EntitiesDbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            var list = entities.ToList();
            ((DbSet<TEntity>)EntitiesDbSet).RemoveRange(list);
        }

        public void AddOrUpdate(TEntity entity)
        {
            EntitiesDbSet.AddOrUpdate(entity);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
