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
    class MbankMock : TheoryData<Company>
    {
        private static Lazy<Company> Mock => new Lazy<Company>(() => new Company
        {
            Ticker = nameof(Properties.Resources.MBANK),
            Quotes = Encoding.UTF8.GetString(Properties.Resources.MBANK).DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList()
        });

        public MbankMock()
        {
            Add(Mock.Value);
        }
    }
}
