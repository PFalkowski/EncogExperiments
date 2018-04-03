using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StocksData.Model;

namespace Services
{
    public class StocksBulkDeserializer
    {
        public IStocksDeserializer Deserializer { get; }

        public StocksBulkDeserializer(IStocksDeserializer deserializer)
        {
            Deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }


        public List<Company> Deserialize(Dictionary<string, string> files)
        {
            var allStocks = new List<Company>(files.Count);

            Parallel.ForEach(files,
                delegate (KeyValuePair<string, string> file)
                {
                    allStocks.Add(Deserializer.Deserialize(file.Value));
                });

            return allStocks;
        }
    }
}
