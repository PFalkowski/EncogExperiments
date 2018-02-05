using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using StocksData.Mappings;
using StocksData.Model;

namespace StocksData.Contexts
{
    public class StockNhContext : INhContext
    {
        public ISessionFactory SessionFactory { get; }
        public StockNhContext(string connectionString)
        {
            SessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012
                        .ConnectionString(connectionString).ShowSql())
                    .Mappings(m =>
                        m.FluentMappings
                            .Add<Company>()
                            .Add<StockQuote>()).BuildSessionFactory();

        }
    }
}
