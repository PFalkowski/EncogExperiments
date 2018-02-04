using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using StocksData.Mappings;

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
                            .Add<StockQuoteMap>()
                            .Add<CompanyMap>()).BuildSessionFactory();

        }
    }
}
