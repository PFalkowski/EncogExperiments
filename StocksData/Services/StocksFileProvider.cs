using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksData.Model;

namespace StocksData.Services
{
    public class StocksFileProvider
    {
        public IOService IoLayer { get; set; }
        public StocksBulkDeserializer DeserializationService { get; set; }

        public List<Company> ReadStocksFrom(string directory, string pattern = "*.*")
        {
            var directorySvc = IoLayer ?? new IOService();
            var deserializationSvc = DeserializationService ?? throw new NullReferenceException(nameof(DeserializationService));
            var stocksRaw = directorySvc.ReadDirectory(directory, pattern);

            return deserializationSvc.Deserialize(stocksRaw);
        }
    }
}
