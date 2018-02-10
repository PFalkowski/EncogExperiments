using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using StocksData.Mappings;

namespace StocksData.Contexts
{
    public class StockNhContextModelUpdate : INhContext
    {
        public ISessionFactory SessionFactory { get; }
        public StockNhContextModelUpdate(string connectionString)
        {
            SessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(connectionString)
                    //.ShowSql
                )
                .Mappings(m =>
                    m.FluentMappings
                            .AddFromAssemblyOf<StockQuoteMap>()
                        //.Add<StockQuoteMap>()
                        //.Add<CompanyMap>()
                    )
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .BuildSessionFactory();

        }
    }
}
