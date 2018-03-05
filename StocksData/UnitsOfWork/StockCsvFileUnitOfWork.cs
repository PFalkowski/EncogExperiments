using System.Threading.Tasks;
using StocksData.Contexts;
using StocksData.Model;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public sealed class StockCsvUnitOfWork : IUnitOfWork
    {
        private readonly StockCsvContext<Company> _context;
        public CsvRepo<Company> Repository { get; set; }

        public StockCsvUnitOfWork(StockCsvContext<Company> context)
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
