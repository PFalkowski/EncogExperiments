using System.Collections.Generic;
using Encog.ML.Data.Basic;
using StocksData.Model;

namespace StocksData.Adapters
{
    public class StockQuoteToBasicMLDataSetAvgConverter
    {
        
        public BasicMLDataSet Convert(List<StockQuote> input)
        {
            var dataset = new BasicMLDataSet();
            var list = input as List<StockQuote>;
            for (var i = 2; i < list?.Count; ++i)
            {
                var openChange = (list[i - 1].Open - list[i - 2].Open) / list[i - 2].Open;
                var highChange = (list[i - 1].High - list[i - 2].High) / list[i - 2].High;
                var lowChange = (list[i - 1].Low - list[i - 2].Low) / list[i - 2].Low;
                var closeChange = (list[i - 1].Close - list[i - 2].Close) / list[i - 2].Close;
                double avgVolatilityChange;
                if ((list[i - 2].High - list[i - 2].Low) == 0)
                { avgVolatilityChange = 0.0; }
                else
                { avgVolatilityChange = ((list[i - 1].High - list[i - 1].Low) - (list[i - 2].High - list[i - 2].Low)) / (list[i - 2].High - list[i - 2].Low); }

                //var volChange = list[i - 1].Close - list[i - 2].Close / list[i - 2].Close;
                var inputQuote = new BasicMLData(new[] { openChange, highChange, lowChange, closeChange, avgVolatilityChange });
                var expValue = (list[i - 1].High - list[i - 1].Low) == 0.0
                    ? 0.0
                    : ((list[i].High - list[i].Low) - (list[i - 1].High - list[i - 1].Low)) /
                      (list[i - 1].High - list[i - 1].Low);
                var expected = new BasicMLData(new double[] { expValue });
                dataset.Add(inputQuote, expected);
            }

            return dataset;
        }
    }
}
