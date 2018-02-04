using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NHibernate.Linq;
using StocksData.Model;

namespace StocksData.Mappings
{

    public class CompanyMap : ClassMap<Company>
    {
        public CompanyMap()
        {
            Id(x => x.Ticker).Column("Ticker");
            HasMany<StockQuote>(x => x.Quotes)
                .Cascade.All()
                .Table(nameof(Company));
            //Schema("dbo");
        }
    }
}
