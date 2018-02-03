using StocksData.Models;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public interface IStockUnitOfWork : IUnitOfWork
    {
        IRepository<Company> StockRepository { get; set; }
    }
}