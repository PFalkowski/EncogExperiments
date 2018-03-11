using Extensions.Serialization;
using StocksData.Mappings;
using StocksData.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StocksData.Test.Mocks
{
   public  class _11BitMock : TheoryData<Company>
    {
        private static Lazy<Company> Mock => new Lazy<Company>(() => new Company
        {
            Ticker = nameof(Properties.Resources._11BIT),
            Quotes = Encoding.UTF8.GetString(Properties.Resources._11BIT).DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList()
        });

        public _11BitMock()
        {
            Add(Mock.Value);
        }
    }
}
