using System;
using System.Data;
using System.Data.SqlClient;
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
            const string tickerName = "Ticker";
            const string dateName = "Date";
            const string openName = "Open";
            const string highName = "High";
            const string lowName = "Low";
            const string closeName = "Close";
            const string volName = "Volume";

            var quotesTableName = "StockQuotes";

            var mbank = MockStockQuoteProvider.Mbank;

            const string connectionStr = @"server=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketRemoveSpecificStock;Integrated Security=True;";



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
                        new SqlBulkCopyColumnMapping("<TICKER>", tickerName),
                        new SqlBulkCopyColumnMapping("<DTYYYYMMDD>", dateName),
                        new SqlBulkCopyColumnMapping("<OPEN>", openName),
                        new SqlBulkCopyColumnMapping("<HIGH>", highName),
                        new SqlBulkCopyColumnMapping("<LOW>", lowName),
                        new SqlBulkCopyColumnMapping("<CLOSE>", closeName),
                        new SqlBulkCopyColumnMapping("<VOL>", volName)
                    }
                };
                inMemoryTable = new DataTable(quotesTableName)
                {
                    Columns =
                    {
                        new DataColumn(tickerName, typeof(string)),
                        new DataColumn(dateName, typeof(int)),
                        new DataColumn(openName, typeof(double)),
                        new DataColumn(highName, typeof(double)),
                        new DataColumn(lowName, typeof(double)),
                        new DataColumn(closeName, typeof(double)),
                        new DataColumn(volName, typeof(double))
                    }
                };
                foreach (var quote in MockStockQuoteProvider.Mbank.Quotes)
                {
                    var newQuoteRow = inMemoryTable.NewRow();

                    newQuoteRow[tickerName] = quote.Ticker;
                    newQuoteRow[dateName] = quote.Date;
                    newQuoteRow[openName] = quote.Open;
                    newQuoteRow[highName] = quote.High;
                    newQuoteRow[lowName] = quote.Low;
                    newQuoteRow[closeName] = quote.Close;
                    newQuoteRow[volName] = quote.Volume;

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
