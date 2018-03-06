﻿using FluentNHibernate.Mapping;
using StocksData.Model;

namespace StocksData.Mappings
{

    public class StockQuoteNhibernateMap : ClassMap<StockQuote>
    {
        public StockQuoteNhibernateMap()
        {
            Table("StockQuotes");
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