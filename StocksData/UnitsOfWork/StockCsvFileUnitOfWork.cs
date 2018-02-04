using System;
using System.Threading.Tasks;
using StocksData.Contexts;
using StocksData.Models;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public sealed class StockCsvFUnitOfWork : IUnitOfWork
    {
        private readonly StockCsvContextEager<Company> _context;
        public CsvRepo<Company> Repository { get; set; }

        public StockCsvFUnitOfWork(StockCsvContextEager<Company> context)
        {
            _context = context;
            Repository = new CsvRepo<Company>(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public Task CompleteAsync()
        {
            return new Task(() =>
                _context.SaveChanges());
        }

        public void Dispose()
        {

        }
    }
}
