using System.Collections.Generic;
using Encog.ML.Data.Basic;

namespace StocksData.Adapters
{
    public class MatrixToMLData
    {
        public BasicMLDataSet ConvertToHighPred(List<double[]> matrix)
        {
            var dataset = new BasicMLDataSet();

            for (var i = 0; i < matrix[0].Length - 1; ++i)
            {
                dataset.Add(new BasicMLData(new[] { matrix[0][i], matrix[1][i], matrix[2][i], matrix[3][i], matrix[4][i] }), new BasicMLData(new[] { matrix[1][i + 1] }));
            }
            return dataset;
        }

        public BasicMLDataSet ConvertToHighLowPred(List<double[]> matrix)
        {
            var dataset = new BasicMLDataSet();

            for (var i = 0; i < matrix[0].Length - 1; ++i)
            {
                dataset.Add(new BasicMLData(new[] { matrix[0][i], matrix[1][i], matrix[2][i], matrix[3][i], matrix[4][i] }), new BasicMLData(new[] { matrix[1][i + 1], matrix[2][i + 1] }));
            }
            return dataset;
        }
    }
}
