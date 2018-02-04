using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using StocksData.Mappings;

namespace StocksData.Contexts
{
    public class StockNHContextModelUpdate : INhContext
    {
        public ISessionFactory SessionFactory { get; }
        public StockNHContextModelUpdate(string connectionString)
        {
            SessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(connectionString).ShowSql())
                .Mappings(m =>
                    m.FluentMappings
                        .Add<StockQuoteMap>()
                        .Add<CompanyMap>()).
                ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .BuildSessionFactory();

        }
    }
}
