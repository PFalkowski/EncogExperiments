using FluentNHibernate.Mapping;
using StocksData.Model;

namespace StocksData.Mappings
{

    public class StockQuoteNhibernateMap : ClassMap<StockQuote>
    {
        public StockQuoteNhibernateMap()
        {
            Table(Constants.StockQuotesName);

            CompositeId()
                .KeyProperty(x => x.Ticker)
                .KeyProperty(x => x.Date);

            Map(x => x.Open).Not.Nullable().Column(Constants.OpenName);
            Map(x => x.High).Not.Nullable().Column(Constants.HighName);
            Map(x => x.Low).Not.Nullable().Column(Constants.LowName);
            Map(x => x.Close).Not.Nullable().Column(Constants.CloseName);
            Map(x => x.Volume).Not.Nullable().Column(Constants.VolName);
        }
    }
}
