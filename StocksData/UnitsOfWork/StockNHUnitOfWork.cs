using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using StocksData.Contexts;
using StocksData.Models;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public class StockNHUnitOfWork : IUnitOfWork
    {
        private ISession Session { get; }
        public IRepository<Company> StocksRepository { get; }

        public StockNHUnitOfWork(IStockNHContext context)
        {
            Session = context.SessionFactory.
                OpenSession();
            Session.BeginTransaction();
            StocksRepository = new NHRepository<Company>(Session);
        }

        public int Complete()
        {
            Session.Flush();
            Session.Transaction.Commit();
            return 1;
        }

        public async Task<int> CompleteAsync()
        {
            await Session.FlushAsync();
            await Session.Transaction.CommitAsync();
            return 1;
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
