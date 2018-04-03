using System.Linq;
using StocksData.Contexts;
using StocksData.Model;
using StocksData.UnitsOfWork;
using StocksData.Test.Mocks;
using Xunit;

namespace StocksData.Test
{
    public class NhRepositoryTest
    {
        [Theory]
        [ClassData(typeof(MbankMock))]
        public void AddingStockToNhibernateWorks(Company company)
        {
            var expected = company.Quotes.Count;
            var dbName = nameof(AddingStockToNhibernateWorks);
            UnitTestHelper.RecreateLocalDatabase(dbName);
            string connectionStr = $@"server=(localdb)\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True;";

            using (var unitOfWork = new StockNhUnitOfWork(new StockNhContextModelUpdate(connectionStr)))
            {
                unitOfWork.Stocks.Repository.Add(company);
                unitOfWork.Complete();

                var all = unitOfWork.Stocks.Repository.GetAll();
                Assert.Equal(1, all.Count);
                Assert.Equal(expected, all.First().Quotes.Count);
            }
        }

        [Theory]
        [ClassData(typeof(MbankMock))]
        public void RemovingSpecificStockFromNhibernateWorks(Company company)
        {
            var expected = company.Quotes.Count;
            var dbName = nameof(AddingStockToNhibernateWorks);
            UnitTestHelper.RecreateLocalDatabase(dbName);
            string connectionStr = $@"server=(localdb)\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True;";

            using (var unitOfWork = new StockNhUnitOfWork(new StockNhContextModelUpdate(connectionStr)))
            {
                unitOfWork.Stocks.Repository.Add(company);
                unitOfWork.Complete();

                var before = unitOfWork.Stocks.Repository.GetAll();
                Assert.Equal(1, before.Count);
                Assert.Equal(expected, before.First().Quotes.Count);

                unitOfWork.Stocks.Repository.Remove(company);
                unitOfWork.Complete();

                var after = unitOfWork.Stocks.Repository.GetAll();

                Assert.Equal(0, after.Count);
            }
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
