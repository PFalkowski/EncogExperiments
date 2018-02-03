using System.Linq;
using System.Threading.Tasks;
using StocksData.Contexts;
using StocksData.Models;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public sealed class StockDatabaseUnitOfWork : IStockUnitOfWork
    {
        private readonly StockDbContext _context;
        public IRepository<Company> StockRepository { get; set; }

        public StockDatabaseUnitOfWork(StockDbContext context)
        {
            _context = context;
            StockRepository = new Repository<Company>(context);
        }

        public int LastDate()
        {
            return StockRepository.Entities.First().Quotes.Max(e => e.Date);
        }
        
        public int Complete()
        {
            _context.BulkSaveChanges(true);
            return 1; 
        }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
