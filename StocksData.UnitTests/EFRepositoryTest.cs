using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StocksData.Mappings;
using StocksData.Model;
using StocksData.UnitsOfWork;
using StocksData.UnitTests.Mocks;
using Xunit;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using StocksData.Services;

namespace StocksData.UnitTests
{
    public class EfRepositoryTest
    {
        [Fact]
        public void AddStock()
        {
            var mbank = MockStockQuoteProvider.Mbank;
            string connectionStr = $"server=(localdb)\\MSSQLLocalDB;Initial Catalog={nameof(AddStock)};Integrated Security=True;";

            using (var unitOfWork = new StockEfUnitOfWork(new StockEfTestContext(connectionStr)))
            {
                unitOfWork.Stocks.Repository.Add(mbank);
                unitOfWork.Complete();
            }

            using (var connection = new SqlConnection(connectionStr))
            using (var command = new SqlCommand("Select count(*) from [Companies]", connection))
            {
                connection.Open();
                var result = (int)command.ExecuteScalar();
                Assert.Equal(1, result);
            }
        }

        [Fact]
        public void RemoveSpecificStock()
        {
            var mbank = MockStockQuoteProvider.Mbank;

            string connectionStr = $"server=(localdb)\\MSSQLLocalDB;Initial Catalog={nameof(RemoveSpecificStock)};Integrated Security=True;";

            using (var unitOfWork = new StockEfUnitOfWork(new StockEfTestContext(connectionStr)))
            {
                Assert.Equal(0, unitOfWork.Stocks.Repository.Count());

                new CompanyBulkInserter(connectionStr).BulkInsert(mbank);

                Assert.Equal(1, unitOfWork.Stocks.Repository.Count());

                unitOfWork.Stocks.Repository.Remove(mbank);
                unitOfWork.Complete();

                Assert.Equal(0, unitOfWork.Stocks.Repository.Count());
            }

            using (var connection = new SqlConnection(connectionStr))
            using (var command = new SqlCommand("Select count(*) from [Companies]", connection))
            {
                connection.Open();
                var result = (int)command.ExecuteScalar();
                Assert.Equal(0, result);
            }
        }

        //[Fact]
        //public void ReadAllFilesFromDirAndSaveToDbAs()
        //{

        //    CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        //    var allStocks = new IOStocksProvider().ReadStocksFrom(@"C:\Users\John\Downloads\mstcgl", "*.mst");

        //    const string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        //    using (var unitOfWork = new StockEfUnitOfWork(new StockEfContextModelUpdate(connectionStr)))
        //    {
        //        //unitOfWork.StockRepository.AddRange(allStocks);
        //        foreach (var stock in allStocks)
        //        {
        //            unitOfWork.Stocks.Repository.AddOrUpdate(stock);
        //            unitOfWork.Complete();
        //        }
        //        unitOfWork.Complete();
        //    }
        //    //Parallel.ForEach(allFiles, (file) => allStocks.Add(file.Value.DeserializeFromCsv(new StockQuoteCsvClassMap()).ToList()));
        //}
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
