using System.Threading.Tasks;
using StocksData.Contexts;

namespace StocksData.UnitsOfWork
{
    public abstract class EfUnitOfWork : IUnitOfWork
    {
        private StockEfContext Context { get; }

        public EfUnitOfWork(StockEfContext context)
        {
            Context = context;
        }
        
        public virtual void Complete()
        {
            Context.SaveChanges();
        }

        public virtual Task CompleteAsync()
        {
            return Context.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            Context.Dispose();
        }
    }
}
