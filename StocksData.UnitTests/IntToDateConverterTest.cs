using StocksData.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StocksData.UnitTests
{
    public class IntToDateConverterTest
    {
        [Fact]
        public void IntGetsProperlyConvertedToDate()
        {
            var expected = DateTime.FromBinary(-8586814107120401456);
            var input = 20180304;
            var tested = new IntToDateConverter();
            var received = tested.Convert(input);

            Assert.Equal(expected.ToShortDateString(), received.ToShortDateString());
        }

        [Fact]
        public void DateGetsProperlyConvertedToInt()
        {
            var expected = 20180304;
            var input = DateTime.FromBinary(-8586814107120401456);
            var tested = new IntToDateConverter();
            var received = tested.ConvertBack(input);
            
        }
    }
}
