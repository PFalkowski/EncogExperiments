using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Extensions.Serialization;
using StocksData.Adapters;
using StocksData.Contexts;
using StocksData.Models;
using StocksData.Services;
using StocksData.UnitsOfWork;
using Xunit;

namespace StocksData.UnitTests
{
    public class CsvRepoTest
    {
        [Fact]
        public void ReadAllFilesFromDirAndSaveToCsvRepo()
        {
            //CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var allFiles = new IOService().ReadDirectory("C:\\Users\\John\\Downloads\\mstcgl", "*.mst");
            var allStocks = new List<List<StockQuote>>(allFiles.Count);

            //foreach (var file in allFiles)
            //{
            //    var deserialized = file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList();
            //    allStocks.Add(deserialized);
            //}
            Parallel.ForEach(allFiles,
                delegate (KeyValuePair<string, string> file)
                {
                    allStocks.Add(file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList());
                });
            var outputFile = new FileInfo("test23443.txt");
            using (var unitOfWork = new StockCsvFUnitOfWork(new StockCsvContextEager<StockQuote>(outputFile)))
            {
                foreach (var stock in allStocks)
                {
                    unitOfWork.StockRepository.AddRange(stock);
                }
                unitOfWork.Complete();
            }

        }
        [Fact]
        public void GetSpecificRecordFromCsvRepo()
        {
            var allFiles = new IOService().ReadDirectory("C:\\Users\\John\\Downloads\\mstcgl", "*.mst");
            var allStocks = new List<List<StockQuote>>(allFiles.Count);

            Parallel.ForEach(allFiles,
                delegate (KeyValuePair<string, string> file)
                {
                    allStocks.Add(file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList());
                });

            using (var unitOfWork = new StockCsvFUnitOfWork(new StockCsvContextEager<StockQuote>(new FileInfo("test232443.txt"))))
            {
                foreach (var stock in allStocks)
                {
                    unitOfWork.StockRepository.AddRange(stock);
                }

                var oneFile = unitOfWork.StockRepository.Entities.Where(x => x.Ticker == "MBANK").ToList();
                unitOfWork.StockRepository.RemoveRange(oneFile);
                unitOfWork.Complete();
            }
        }
        //[Fact]
        //public void DeleteAllStocksFromCsvRepo()
        //{
        //    var outputFile = new FileInfo("test23443.txt");
        //    var factory = new StockQuotesCsvUnitOfWorkFactory(outputFile);
        //    using (var unitOfWork = factory.GetInstance())
        //    {
        //        unitOfWork.StockRepository.RemoveRange();
        //        unitOfWork.Complete();
        //    }
        //}
        //[Fact]
        //public void RemoveOneStockFromCsvRepo()
        //{
        //    var outputFile = new FileInfo("test23443.txt");
        //    var factory = new StockQuotesCsvUnitOfWorkFactory(outputFile);
        //    using (var unitOfWork = factory.GetInstance())
        //    {
        //        unitOfWork.StockRepository.Remove(unitOfWork.StockRepository.Get());
        //        unitOfWork.Complete();
        //    }
        //}
    }
}
