using System;
using System.Data;
using System.Data.SqlClient;
using StocksData.Mappings;
using StocksData.UnitsOfWork;
using StocksData.UnitTests.Mocks;
using Xunit;

namespace StocksData.UnitTests
{
    public class EfRepositoryTest
    {
        [Fact]
        public void AddStock()
        {
            var mbank = MockStockQuoteProvider.Mbank;
            const string connectionStr = @"server=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketAddStock;Integrated Security=True;";

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

            var quotesTableName = "StockQuotes";

            var mbank = MockStockQuoteProvider.Mbank;

            const string connectionStr = @"server=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;";



            var sqlConnection = default(SqlConnection);
            var sqlBulkCopy = default(SqlBulkCopy);
            var inMemoryTable = default(DataTable);
            try
            {
                sqlConnection = new SqlConnection(connectionStr);
                sqlBulkCopy = new SqlBulkCopy(sqlConnection)
                {
                    DestinationTableName = quotesTableName,
                    ColumnMappings =
                    {
                        new SqlBulkCopyColumnMapping(Constants.TickerName, Constants.TickerName),
                        new SqlBulkCopyColumnMapping(Constants.DateName, Constants.DateName),
                        new SqlBulkCopyColumnMapping(Constants.OpenName, Constants.OpenName),
                        new SqlBulkCopyColumnMapping(Constants.HighName, Constants.HighName),
                        new SqlBulkCopyColumnMapping(Constants.LowName, Constants.LowName),
                        new SqlBulkCopyColumnMapping(Constants.CloseName, Constants.CloseName),
                        new SqlBulkCopyColumnMapping(Constants.VolName, Constants.VolName)
                    }
                };
                inMemoryTable = new DataTable(quotesTableName)
                {
                    Columns = {
                        new DataColumn(Constants.TickerName, typeof(string)),
                        new DataColumn(Constants.DateName, typeof(int)),
                        new DataColumn(Constants.OpenName, typeof(double)),
                        new DataColumn(Constants.HighName, typeof(double)),
                        new DataColumn(Constants.LowName, typeof(double)),
                        new DataColumn(Constants.CloseName, typeof(double)),
                        new DataColumn(Constants.VolName, typeof(double))
                    }
                };
                inMemoryTable.PrimaryKey = new[]
                {
                    inMemoryTable.Columns[0],
                    inMemoryTable.Columns[1]
                };

                foreach (var quote in MockStockQuoteProvider.Mbank.Quotes)
                {
                    var newQuoteRow = inMemoryTable.NewRow();

                    newQuoteRow[Constants.TickerName] = quote.Ticker;
                    newQuoteRow[Constants.DateName] = quote.Date;
                    newQuoteRow[Constants.OpenName] = quote.Open;
                    newQuoteRow[Constants.HighName] = quote.High;
                    newQuoteRow[Constants.LowName] = quote.Low;
                    newQuoteRow[Constants.CloseName] = quote.Close;
                    newQuoteRow[Constants.VolName] = quote.Volume;

                    inMemoryTable.Rows.Add(newQuoteRow);
                }
                sqlConnection.Open();
                sqlBulkCopy.WriteToServer(inMemoryTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                sqlConnection?.Dispose();
                inMemoryTable?.Dispose();
                ((IDisposable)sqlBulkCopy)?.Dispose();
            }


            using (var unitOfWork = new StockEfUnitOfWork(new StockEfTestContext(connectionStr)))
            {
                unitOfWork.Stocks.Repository.Remove(mbank);
                unitOfWork.Complete();
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
