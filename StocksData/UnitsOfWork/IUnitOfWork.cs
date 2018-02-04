using System;
using System.Threading.Tasks;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();
        Task CompleteAsync();
    }
}
