using System.Collections.Generic;
using StocksData.Model;

namespace StocksData.Repositories
{
    public class StockRepository : IRepository<Company>
    {
        public IRepository<Company> Repository { get; }
        public StockRepository(IRepository<Company> repository)
        {
            Repository = repository;
        }
        public IEnumerable<Company> Entities => Repository.Entities;

        public void Add(Company entity) => Repository.Add(entity);

        public void AddOrUpdate(Company entity) => Repository.AddOrUpdate(entity);

        public void AddRange(IEnumerable<Company> entities) => Repository.AddRange(entities);
        
        public IList<Company> GetAll() => Repository.GetAll();

        public void Remove(Company entity) => Repository.Remove(entity);

        public void RemoveRange(IEnumerable<Company> entities) => Repository.RemoveRange(entities);
    }
}
