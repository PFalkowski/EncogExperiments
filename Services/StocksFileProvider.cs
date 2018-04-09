using System;
using System.Collections.Generic;
using StocksData.Model;

namespace Services
{
    public class StocksFileProvider
    {
        public IoService IoLayer { get; set; }
        public StocksBulkDeserializer DeserializationService { get; set; }

        public List<Company> ReadStocksFrom(string directory, string pattern = "*.*")
        {
            var directorySvc = IoLayer ?? new IoService();
            var deserializationSvc = DeserializationService ?? throw new NullReferenceException(nameof(DeserializationService));
            var stocksRaw = directorySvc.ReadDirectory(directory, pattern);

            return deserializationSvc.Deserialize(stocksRaw);
        }
    }
}
