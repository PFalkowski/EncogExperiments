using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Extensions.Serialization;
using StocksData.Contexts;
using StocksData.Mappings;
using StocksData.Model;
using StocksData.Services;
using StocksData.UnitsOfWork;
using StocksData.UnitTests.Mocks;
using Xunit;

namespace StocksData.UnitTests
{
    public class NhRepositoryTest
    {
        [Fact]
        public void AddStock()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var mbank = MockStockQuoteProvider.Mbank;
            const string connectionStr = @"server=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketAddStock;Integrated Security=True;";

            using (var unitOfWork = new StockNhUnitOfWork(new StockNhContextModelUpdate(connectionStr)))
            {
                unitOfWork.Stocks.Repository.Add(mbank);
                unitOfWork.Complete();
            }
        }

        [Fact]
        public void RemoveSpecificStock()
        {
            var mbank = MockStockQuoteProvider.Mbank;
            const string connectionStr = @"server=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketRemoveSpecificStock;Integrated Security=True;";

            using (var unitOfWork = new StockNhUnitOfWork(new StockNhContextModelUpdate(connectionStr)))
            {
                unitOfWork.Stocks.Repository.Remove(mbank);
                unitOfWork.Complete();
            }
        }

        [Fact]
        public void ReadAllFilesFromDirAndSaveToDbAsCompanyStockQuotes()
        {

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var allFiles = new IOService().ReadDirectory("C:\\Users\\John\\Downloads\\mstcgl", "*.mst");
            var allStocks = new List<Company>(allFiles.Count);

            Parallel.ForEach(allFiles,
                delegate (KeyValuePair<string, string> file)
                {
                    var deserializedQuotes = file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList();
                    if (!string.Equals(Path.GetFileNameWithoutExtension(file.Key), deserializedQuotes.First().Ticker, StringComparison.InvariantCulture)) throw new Exception($"{Path.GetFileNameWithoutExtension(file.Key)} != {deserializedQuotes.First().Ticker}");

                    allStocks.Add(new Company { Ticker = deserializedQuotes.First().Ticker, Quotes = deserializedQuotes });
                });
            const string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using (var unitOfWork = new StockNhUnitOfWork(new StockNhContextModelUpdate(connectionStr)))
            {
                //unitOfWork.StockRepository.AddRange(allStocks);
                foreach (var stock in allStocks)
                {
                    unitOfWork.Stocks.Repository.AddOrUpdate(stock);
                    unitOfWork.Complete();
                }
                unitOfWork.Complete();
            }
            //Parallel.ForEach(allFiles, (file) => allStocks.Add(file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap()).ToList()));

        }
        //[Fact]
        //public void GetSpecificStock()
        //{
        //    const string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        //    using (var unitOfWork = new StockDatabaseUnitOfWork(new StockDbContextModelUpdate(connectionStr)))
        //    {
        //        unitOfWork.StockRepository.GetAll((x) => x.Ticker == "MBANK");
        //        unitOfWork.Complete();
        //    }
        //    //Parallel.ForEach(allFiles, (file) => allStocks.Add(file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap()).ToList()));

        //}
    }
}
