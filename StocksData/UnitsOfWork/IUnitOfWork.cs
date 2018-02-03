using System;
using System.Threading.Tasks;

namespace StocksData.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();
        Task<int> CompleteAsync();
    }
}
