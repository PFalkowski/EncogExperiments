using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Extensions.Serialization;
using StocksData.Mappings;
using StocksData.Model;

namespace StocksData.Services
{
    public class IOStocksProvider
    {
        public List<Company> ReadStocksFrom(string directory, string pattern = "*.mst",
            SearchOption searchDepth = SearchOption.TopDirectoryOnly)
        {
            return ReadStocksFrom(new DirectoryInfo(directory), pattern, searchDepth);
        }

        public List<Company> ReadStocksFrom(DirectoryInfo dir, string pattern = "*.mst", SearchOption searchDepth = SearchOption.TopDirectoryOnly)
        {

            var allFiles = new IOService().ReadDirectory(dir, pattern, searchDepth);
            var allStocks = new List<Company>(allFiles.Count);

            Parallel.ForEach(allFiles,
                delegate (KeyValuePair<string, string> file)
                {
                    var deserializedQuotes = file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList();
                    if (file.Key != deserializedQuotes.First().Ticker) throw new Exception($"{file.Key} != {deserializedQuotes.First().Ticker}");

                    allStocks.Add(new Company
                    {
                        Ticker = deserializedQuotes.First().Ticker,
                        Quotes = deserializedQuotes
                    });
                });

            return allStocks;
        }
    }
}
