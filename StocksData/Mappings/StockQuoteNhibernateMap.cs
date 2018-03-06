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
            Map(x => x.Open).Column(Constants.OpenName);
            Map(x => x.High).Column(Constants.HighName);
            Map(x => x.Low).Column(Constants.LowName);
            Map(x => x.Close).Column(Constants.CloseName);
            Map(x => x.Volume).Column(Constants.VolName);
        }
    }
}
