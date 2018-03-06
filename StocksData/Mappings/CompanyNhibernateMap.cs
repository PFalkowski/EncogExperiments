using FluentNHibernate.Mapping;
using StocksData.Model;

namespace StocksData.Mappings
{

    public class CompanyNhibernateMap : ClassMap<Company>
    {
        public CompanyNhibernateMap()
        {
            Table("Companies");
            Id(x => x.Ticker);
            //Map(x => x.Ticker);
            HasMany(x => x.Quotes)//.Table("StockQuote")
            .KeyColumn("Ticker")
            //.ForeignKeyConstraintName("CompanyQuotesConstrint")
            //.Not.Inverse()
            //.Inverse()
            .Cascade.All();
        }
    }
}
