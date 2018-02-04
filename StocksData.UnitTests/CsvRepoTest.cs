using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Extensions.Serialization;
using StocksData.Adapters;
using StocksData.Contexts;
using StocksData.Mappings;
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

            var allFiles = new IOService().ReadDirectory(@"C:\Users\John\Downloads\mstcgl", "*.mst");
            var allStocks = new List<Company>(allFiles.Count);

            Parallel.ForEach(allFiles,
                delegate (KeyValuePair<string, string> file)
                {
                    var deserializedQuotes = file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList();
                    //if (deserializedQuotes.Count < 200) return;
                    if (file.Key != deserializedQuotes.First().Ticker) throw new Exception($"{file.Key} != {deserializedQuotes.First().Ticker}");

                    allStocks.Add(new Company
                    {
                        Ticker = deserializedQuotes.First().Ticker,
                        Quotes = deserializedQuotes
                    });
                });
            var outputFile = new FileInfo("test23443.txt");
            using (var unitOfWork = new StockCsvFUnitOfWork(new StockCsvContextEager<Company>(outputFile)))
            {
                foreach (var stock in allStocks)
                {
                    unitOfWork.Repository.AddOrUpdate(stock);
                    unitOfWork.Complete();
                }
            }

        }
        [Fact]
        public void GetSpecificRecordFromCsvRepo()
        {

            var allFiles = new IOService().ReadDirectory(@"C:\Users\John\Downloads\mstcgl", "*.mst");
            var allStocks = new List<Company>(allFiles.Count);

            Parallel.ForEach(allFiles,
                delegate (KeyValuePair<string, string> file)
                {
                    var deserializedQuotes = file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList();
                    if (deserializedQuotes.Count < 200) return;
                    if (file.Key != deserializedQuotes.First().Ticker) throw new Exception($"{file.Key} != {deserializedQuotes.First().Ticker}");

                    allStocks.Add(new Company
                    {
                        Ticker = deserializedQuotes.First().Ticker,
                        Quotes = deserializedQuotes
                    });
                });
            var outputFile = new FileInfo("test23443.txt");
            using (var unitOfWork = new StockCsvFUnitOfWork(new StockCsvContextEager<Company>(outputFile)))
            {
                unitOfWork.Repository.AddRange(allStocks);

                var oneStock = unitOfWork.Repository.Entities.FirstOrDefault(x => x.Ticker == "MBANK");
                unitOfWork.Repository.Remove(oneStock);

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
