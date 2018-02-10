using System;
using System.Collections.Generic;
using StocksData.Contexts;

namespace StocksData.Repositories
{
    public class CsvRepo<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public StockCsvContext<TEntity> Context { get; set; }
        public List<TEntity> EntitiesList { get; set; }

        public IEnumerable<TEntity> Entities => EntitiesList;

        public CsvRepo(StockCsvContext<TEntity> context)
        {
            Context = context;
            EntitiesList = context.Entities;
        }

        public void Add(TEntity entity)
        {
            EntitiesList.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            EntitiesList.AddRange(entities);
        }

        public TEntity Get(object id)
        {
            return EntitiesList.Find(x => x.Equals(id));
        }

        public List<TEntity> GetAll(Predicate<TEntity> match)
        {
            return EntitiesList.FindAll(match);
        }

        public IList<TEntity> GetAll()
        {
            return EntitiesList;
        }

        public void Remove(TEntity entity)
        {
            EntitiesList.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public void RemoveAll()
        {
            EntitiesList.RemoveAll(x => true);
        }

        public void AddOrUpdate(TEntity entity)
        {
            var found = Get(entity);
            if (found != null)
            {
                found = entity;
            }
            else
            {
                Add(entity);
            }
        }

        public void AddRangeBulk(IEnumerable<TEntity> entities)
        {
            AddRange(entities);
        }

        public void AddBulk(TEntity entity)
        {
            Add(entity);
        }
    }
}
