using System;

namespace StocksData.Converters
{
    public interface IIntToDateConverter
    {
        DateTime Convert(int input);
        int ConvertBack(DateTime input);
    }
}