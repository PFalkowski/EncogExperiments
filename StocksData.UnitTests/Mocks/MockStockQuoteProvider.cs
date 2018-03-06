using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Extensions.Serialization;
using StocksData.Mappings;
using StocksData.Model;

namespace StocksData.UnitTests.Mocks
{
    public static class MockStockQuoteProvider
    {
        public static Lazy<Dictionary<string, Company>> Mocks => new Lazy<Dictionary<string, Company>>(
            () =>
                {
                    return new Dictionary<string, Company> {
                     { "MBANK", new Company { Ticker = "MBANK", Quotes = Encoding.UTF8.GetString(Properties.Resources.MBANK).DeserializeFromCsv<StockQuote>(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList() }},
                     { "11BIT",  new Company { Ticker = "11BIT", Quotes = Encoding.UTF8.GetString(Properties.Resources._11BIT).DeserializeFromCsv<StockQuote>(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList() }},
                     { "CDPROJEKT", new Company { Ticker = "CDPROJEKT", Quotes = Encoding.UTF8.GetString(Properties.Resources.CDPROJEKT).DeserializeFromCsv<StockQuote>(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList() }}
                };
            });
    }
}
