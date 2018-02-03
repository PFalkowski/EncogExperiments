using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Extensions.Serialization;
using StocksData.Adapters;
using StocksData.Contexts;
using StocksData.Models;
using StocksData.Services;
using StocksData.UnitsOfWork;
using StocksData.UnitTests.Mocks;
using Xunit;

namespace StocksData.UnitTests
{
    public class RepositoryTest
    {
        [Fact]
        public void RepositoryCanBeCreatedWithDbContext()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var mbank = MockStockQuoteProvider.Mbank;
            const string connectionStr = @"server=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;";

            using (var unitOfWork = new StockDatabaseUnitOfWork(new StockDbContext(connectionStr)))
            {
                unitOfWork.StockRepository.AddRange(mbank);
                unitOfWork.Complete();
            }
        }


        [Fact]
        public void Test()
        {
            var mbank = MockStockQuoteProvider.Mbank;
            var res = mbank.Count;
        }


        [Fact]
        public void ReadAllFilesFromDirAndSaveToDb()
        {

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var allFiles = new IOService().ReadDirectory("C:\\Users\\John\\Downloads\\mstcgl", "*.mst");
            var allStocks = new List<List<StockQuote>>(allFiles.Count);

            Parallel.ForEach(allFiles,
                delegate (KeyValuePair<string, string> file)
                {
                    allStocks.Add(file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap(), CultureInfo.InvariantCulture).ToList());
                });
            const string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            
            using (var unitOfWork = new StockDatabaseUnitOfWork(new StockDbContextModelUpdate(connectionStr)))
            {
                for (var i = 0; i < allStocks.Count; ++i)
                {
                    unitOfWork.StockRepository.AddRange(allStocks[i]);
                    //if (i % 10 == 0)
                        unitOfWork.Complete();
                }
                unitOfWork.Complete();
            }
            //Parallel.ForEach(allFiles, (file) => allStocks.Add(file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap()).ToList()));

        }
        [Fact]
        public void GetSpecificStock()
        {
            const string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using (var unitOfWork = new StockDatabaseUnitOfWork(new StockDbContextModelUpdate(connectionStr)))
            {
                unitOfWork.StockRepository.GetAll((x) => x.Ticker == "MBANK");
                unitOfWork.Complete();
            }
            //Parallel.ForEach(allFiles, (file) => allStocks.Add(file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap()).ToList()));

        }
    }
}
