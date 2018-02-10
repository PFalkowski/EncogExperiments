using FluentNHibernate.Mapping;
using StocksData.Model;

namespace StocksData.Mappings
{

    public class CompanyMap : ClassMap<Company>
    {
        public CompanyMap()
        {
            Id(x => x.Ticker);
            //Map(x => x.Ticker);
            HasMany(x => x.Quotes)
            .KeyColumn("Ticker")
            //.Inverse()
            .Cascade.All();
        }
    }
}
