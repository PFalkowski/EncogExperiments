using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksData.Converters
{
    public class IntToDateConverter : IIntToDateConverter
    {
        public DateTime Convert(int input)
        {
            return DateTime.ParseExact(input.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
        }
        public int ConvertBack(DateTime input)
        {
            return int.Parse($"{input.Year}{input.Month}{input.Day}");
        }
    }
}
