using StocksData.Contexts;
using StocksData.Model;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public class StockNhUnitOfWork : NhUnitOfWork
    {
        public StockRepository Stocks { get; }
        public StockNhUnitOfWork(INhContext context) : base(context)
        {
            Stocks = new StockRepository(new NhRepository<Company>(context.SessionFactory.OpenSession()));
        }
    }
}
