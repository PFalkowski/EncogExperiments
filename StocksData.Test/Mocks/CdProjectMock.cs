using Extensions.Serialization;
using StocksData.Mappings;
using StocksData.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Xunit;

namespace StocksData.Test.Mocks
{
   public class CdProjectMock : TheoryData<Company>
    {
        private static Lazy<Company> Mock => new Lazy<Company>(() => new Company
        {
            Ticker = nameof(Properties.Resources.CDPROJEKT),
            Quotes = Encoding.UTF8.GetString(Properties.Resources.CDPROJEKT).DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList()
        });

        public CdProjectMock()
        {
            Add(Mock.Value);
        }
    }
}
