using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public void AddingStockToNhibernateWorks()
        {
            var dbName = nameof(AddingStockToNhibernateWorks);
            UnitTestHelper.RecreateLocalDatabase(dbName);
            var company = MockStockQuoteProvider.Mocks.Value["11BIT"];
            string connectionStr = $@"server=(localdb)\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True;";

            using (var unitOfWork = new StockNhUnitOfWork(new StockNhContextModelUpdate(connectionStr)))
            {
                unitOfWork.Stocks.Repository.Add(company);
                unitOfWork.Complete();

                var all = unitOfWork.Stocks.Repository.GetAll();
                Assert.Equal(1, all.Count);
                Assert.Equal(1814, all.First().Quotes.Count);
            }
        }

        [Fact]
        public void RemovingSpecificStockFromNhibernateWorks()
        {
            var dbName = nameof(AddingStockToNhibernateWorks);
            UnitTestHelper.RecreateLocalDatabase(dbName);
            var company = MockStockQuoteProvider.Mocks.Value["11BIT"];
            string connectionStr = $@"server=(localdb)\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True;";

            using (var unitOfWork = new StockNhUnitOfWork(new StockNhContextModelUpdate(connectionStr)))
            {
                unitOfWork.Stocks.Repository.Add(company);
                unitOfWork.Complete();

                var before = unitOfWork.Stocks.Repository.GetAll();
                Assert.Equal(1, before.Count);
                Assert.Equal(1814, before.First().Quotes.Count);

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
