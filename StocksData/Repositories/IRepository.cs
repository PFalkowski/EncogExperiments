using System.Collections.Generic;

namespace StocksData.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Entities { get; }
        //TEntity Get(Expression<Func<TEntity, bool>> predicate);
        //IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        IList<TEntity> GetAll();
        void Add(TEntity entity);
        void AddOrUpdate(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}