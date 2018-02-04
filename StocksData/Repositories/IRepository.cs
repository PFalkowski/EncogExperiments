using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StocksData.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Entities { get; }
        TEntity Get(object id);
        IList<TEntity> GetAll();
        void Add(TEntity entity);
        void AddOrUpdate(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}