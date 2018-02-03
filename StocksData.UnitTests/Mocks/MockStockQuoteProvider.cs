using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions.Serialization;
using StocksData.Adapters;
using StocksData.Models;

namespace StocksData.UnitTests.Mocks
{
    public static class MockStockQuoteProvider
    {
        private static readonly Lazy<List<StockQuote>> MbankLazy = new Lazy<List<StockQuote>>(
            () => Encoding.UTF8.GetString(Properties.Resources.MBANK).DeserializeFromCsv<StockQuote>(new StockQuoteCsvClassMap()).ToList());

        public static Company Mbank => new Company {Ticker = "MBANK", Quotes = MbankLazy.Value};
    }
}
