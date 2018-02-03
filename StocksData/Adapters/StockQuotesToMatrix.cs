using System.Collections.Generic;
using StocksData.Models;

namespace StocksData.Adapters
{
    public class StockQuotesToMatrix
    {
        public List<double[]> Convert(List<StockQuote> input)
        {
            var open = new double[input.Count];
            var high = new double[input.Count];
            var low = new double[input.Count];
            var close = new double[input.Count];
            var vol = new double[input.Count];

            for (var i = 0; i < input.Count; ++i)
            {
                open[i] = input[i].Open;
                high[i] = input[i].High;
                low[i] = input[i].Low;
                close[i] = input[i].Close;
                vol[i] = input[i].Volume;
            }

            return new List<double[]> { open, high, low, close, vol };
        }
    }
}
