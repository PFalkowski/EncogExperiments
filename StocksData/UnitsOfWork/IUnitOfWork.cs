using System;
using System.Threading.Tasks;

namespace StocksData.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();
        Task CompleteAsync();
    }
}
