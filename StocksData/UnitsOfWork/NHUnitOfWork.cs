using System.Threading.Tasks;
using NHibernate;
using StocksData.Contexts;

namespace StocksData.UnitsOfWork
{
    public abstract class NhUnitOfWork : IUnitOfWork
    {
        public ISession Session { get; }

        public NhUnitOfWork(INhContext context)
        {
            Session = context.SessionFactory.
                OpenSession();
            Session.FlushMode = FlushMode.Commit;
            //Session.BeginTransaction();
        }

        public virtual void Complete()
        {
            //Session.Transaction.Commit();
            Session.Flush();
        }

        public virtual async Task CompleteAsync()
        {
            await Session.Transaction.CommitAsync();
            await Session.FlushAsync();
        }

        public virtual void Dispose()
        {
            Session.Dispose();
        }
    }
}
