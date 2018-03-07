using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
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
                        .ConnectionString(connectionString).ShowSql())
                    .Mappings(m =>
                        m.FluentMappings
                            .Add<CompanyNhibernateMap>()
                            .Add<StockQuoteNhibernateMap>()
                            )
                            .ExposeConfiguration(c => SchemaMetadataUpdater.QuoteTableAndColumns(c, new MsSql2012Dialect()))
                            .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false))
                            .BuildSessionFactory();

        }
    }
}
