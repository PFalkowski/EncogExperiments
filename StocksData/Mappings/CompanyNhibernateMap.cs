using FluentNHibernate.Mapping;
using StocksData.Model;

namespace StocksData.Mappings
{

    public class CompanyNhibernateMap : ClassMap<Company>
    {
        public CompanyNhibernateMap()
        {
            Table(Constants.CompaniesName);
            Id(x => x.Ticker);
            HasMany(x => x.Quotes)
            .KeyColumn(Constants.TickerName)
            .Cascade.All();
        }
    }
}
