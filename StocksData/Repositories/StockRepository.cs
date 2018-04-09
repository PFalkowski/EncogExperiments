using System.Collections.Generic;
using System.Linq;
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

        public int Count() => Entities.Count();

        public void Add(Company entity) => Repository.Add(entity);

        public void AddOrUpdate(Company entity) => Repository.AddOrUpdate(entity);

        public void AddRange(IEnumerable<Company> entities) => Repository.AddRange(entities);
        
        public IList<Company> GetAll() => Repository.GetAll();

        //public Company Get(Expression<Func<Company, bool>> predicate) => Entities.FirstOrDefault(predicate);

        //public IEnumerable<Company> GetAll(Expression<Func<Company, bool>> predicate) => Entities.Where(predicate);
        public void Remove(Company entity) => Repository.Remove(entity);

        public void RemoveRange(IEnumerable<Company> entities) => Repository.RemoveRange(entities);

        public int QuotesCount() => Entities.Sum(x => x.Quotes.Count);

        public void Dispose()
        {
            Repository?.Dispose();
        }
    }
}
