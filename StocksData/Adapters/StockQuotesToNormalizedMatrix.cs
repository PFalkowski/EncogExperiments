using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Encog.ML.Data.Basic;
using Encog.Util.Arrayutil;
using StocksData.Model;

namespace StocksData.Adapters
{
    public class StockQuotesToNormalizedMatrix
    {
        public List<double[]> Convert(List<StockQuote> input)
        {
            var matrixConvert = new StockQuotesToMatrix();
            var matrix = matrixConvert.Convert(input);
            var max = matrix[1].Max();
            var min = matrix[2].Min();
            var priceNormalizer = new NormalizeArray((int)Math.Floor(min), (int)Math.Ceiling(max))
            {
                NormalizedHigh = 1.0,
                NormalizedLow = 0.0
            };
            var volMin = matrix[4].Min();
            var volMax = matrix[4].Max();
            var volNormalizer = new NormalizeArray((int)Math.Floor(volMin), (int)Math.Ceiling(volMax))
            {
                NormalizedHigh = 1.0,
                NormalizedLow = 0.0
            };
            var openNormalized = default(double[]);
            var highNormalized = default(double[]);
            var lowNormalized = default(double[]);
            var closeNormalized = default(double[]);
            var volNormalized = default(double[]);
            Parallel.Invoke(
                () => openNormalized = priceNormalizer.Process(matrix[0]),
                () => highNormalized = priceNormalizer.Process(matrix[1]),
                () => lowNormalized = priceNormalizer.Process(matrix[2]),
                () => closeNormalized = priceNormalizer.Process(matrix[3]),
                () => volNormalized = volNormalizer.Process(matrix[4]));
            return new List<double[]> { openNormalized, highNormalized, lowNormalized, closeNormalized, volNormalized };
        }
    }
}
