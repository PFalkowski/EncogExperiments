using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions.Serialization;
using StocksData.Mappings;
using StocksData.Model;

namespace StocksData.Services
{
    public class StocksBulkDeserializer
    {
        public List<Company> Deserialize(Dictionary<string, string> files)
        {
            var allStocks = new List<Company>(files.Count);

            Parallel.ForEach(files,
                delegate (KeyValuePair<string, string> file)
                {
                    var deserializedQuotes = file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList();
                    var companyName = Path.GetFileNameWithoutExtension(file.Key);
                    if (companyName != deserializedQuotes.First().Ticker) throw new Exception($"{companyName} != {deserializedQuotes.First().Ticker}");

                    allStocks.Add(new Company
                    {
                        Ticker = companyName,
                        Quotes = deserializedQuotes
                    });
                });

            return allStocks;
        }
    }
}
