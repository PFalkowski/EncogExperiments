using System;
using System.Threading.Tasks;
using StocksData.Contexts;
using StocksData.Models;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public sealed class StockCsvFUnitOfWork : IStockUnitOfWork
    {
        private readonly StockCsvContextEager<StockQuote> _context;
        public IRepository<StockQuote> StockRepository { get; set ; }

        public StockCsvFUnitOfWork(StockCsvContextEager<StockQuote> context)
        {
            _context = context;
            StockRepository = new CsvRepo<StockQuote>(context);
        }

        public int Complete()
        {
            _context.SaveChanges();
            return 1;
        }

        public Task<int> CompleteAsync()
        {
            _context.SaveChanges();
            return new Task<int>(() => 1);
        }

        public void Dispose()
        {
            
        }
    }
}
