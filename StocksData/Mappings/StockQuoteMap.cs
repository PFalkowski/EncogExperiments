using FluentNHibernate.Mapping;
using StocksData.Models;

namespace StocksData.Mappings
{

    public class StockQuoteMap : ClassMap<StockQuote>
    {
        public StockQuoteMap()
        {
            //Schema("dbo");
            CompositeId()
                .KeyProperty(x => x.Ticker)
                .KeyProperty(x => x.Date);
            Map(x => x.Open).Column("Open");
            Map(x => x.High).Column("High");
            Map(x => x.Low).Column("Low");
            Map(x => x.Close).Column("Close");
            Map(x => x.Volume).Column("Volume");
        }
    }
}
