using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksData.Services;
using StocksData.UnitsOfWork;
using StocksData.UnitTests.Mocks;
using Xunit;

namespace StocksData.UnitTests
{
    public class BulkInserterTest
    {
        [Fact]
        public void BulkInsertOneStock()
        {
            var mbank = MockStockQuoteProvider.Mbank;
            string connectionStr = $"server=(localdb)\\MSSQLLocalDB;Initial Catalog={nameof(BulkInsertOneStock)};Integrated Security=True;";

            var context = new StockEfTestContext(connectionStr);
            context.DropDbIfExists();
            context.CreateDbIfNotExists();

            // Act

            var tested = new CompanyBulkInserter(connectionStr);
            tested.BulkInsert(mbank);

            //Assert

            using (var connection = new SqlConnection(connectionStr))
            using (var command = new SqlCommand("Select Top(1) Ticker from [Companies]", connection))
            {
                connection.Open();
                var result = (string)command.ExecuteScalar();
                Assert.Equal(mbank.Ticker, result);
            }
        }
    }
}
