using System.Linq;
using System.Threading.Tasks;
using StocksData.Contexts;
using StocksData.Models;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public sealed class StockEFUnitOfWork : IStockUnitOfWork
    {
        private StockEFContext Context { get; }
        public IRepository<Company> StockRepository { get; set; }

        public StockEFUnitOfWork(StockEFContext context)
        {
            Context = context;
            StockRepository = new EFRepository<Company>(context);
        }

        public int LastDate()
        {
            return StockRepository.Entities.First().Quotes.Max(e => e.Date);
        }
        
        public int Complete()
        {
            Context.SaveChanges();
            return 1; 
        }

        public Task<int> CompleteAsync()
        {
            return Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
