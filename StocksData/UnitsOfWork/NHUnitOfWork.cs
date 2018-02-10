using System.Threading.Tasks;
using NHibernate;
using StocksData.Contexts;

namespace StocksData.UnitsOfWork
{
    public abstract class NhUnitOfWork : IUnitOfWork
    {
        private ISession Session { get; }

        public NhUnitOfWork(INhContext context)
        {
            Session = context.SessionFactory.
                OpenSession();
            Session.BeginTransaction();
        }

        public virtual void Complete()
        {
            Session.Flush();
            Session.Transaction.Commit();
        }

        public virtual async Task CompleteAsync()
        {
            await Session.FlushAsync();
            await Session.Transaction.CommitAsync();
        }

        public virtual void Dispose()
        {
            Session.Dispose();
        }
    }
}
