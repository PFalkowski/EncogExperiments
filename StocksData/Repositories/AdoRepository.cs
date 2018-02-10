//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using StocksData.Mappings;

//namespace StocksData.Repositories
//{
//    public class AdoRepository<TEntity> : IRepository<TEntity> where TEntity : class 
//    {
//        public SqlConnection DbConnection { get; }

//        public AdoRepository(SqlConnection connection)
//        {
//            DbConnection = connection;
//        }
//        public IEnumerable<TEntity> Entities => throw new NotImplementedException();

//        public void Add(TEntity entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void AddOrUpdate(TEntity entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void AddRange(IEnumerable<TEntity> entities)
//        {
//            var sqlConnection = default(DbConnection);
//            var sqlBulkCopy = default(SqlBulkCopy);
//            var inMemoryTable = default(DataTable);
//            try
//            {
//                sqlBulkCopy = new SqlBulkCopy(DbConnection)
//                {
//                    DestinationTableName = quotesTableName,
//                    ColumnMappings =
//                    {
//                        new SqlBulkCopyColumnMapping(Constants.TickerName, Constants.TickerName),
//                        new SqlBulkCopyColumnMapping(Constants.DateName, Constants.DateName),
//                        new SqlBulkCopyColumnMapping(Constants.OpenName, Constants.OpenName),
//                        new SqlBulkCopyColumnMapping(Constants.HighName, Constants.HighName),
//                        new SqlBulkCopyColumnMapping(Constants.LowName, Constants.LowName),
//                        new SqlBulkCopyColumnMapping(Constants.CloseName, Constants.CloseName),
//                        new SqlBulkCopyColumnMapping(Constants.VolName, Constants.VolName)
//                    }
//                };
//                inMemoryTable = new DataTable(quotesTableName)
//                {
//                    Columns = {
//                        new DataColumn(Constants.TickerName, typeof(string)),
//                        new DataColumn(Constants.DateName, typeof(int)),
//                        new DataColumn(Constants.OpenName, typeof(double)),
//                        new DataColumn(Constants.HighName, typeof(double)),
//                        new DataColumn(Constants.LowName, typeof(double)),
//                        new DataColumn(Constants.CloseName, typeof(double)),
//                        new DataColumn(Constants.VolName, typeof(double))
//                    }
//                };
//                inMemoryTable.PrimaryKey = new[]
//                {
//                    inMemoryTable.Columns[0],
//                    inMemoryTable.Columns[1]
//                };

//                foreach (var quote in MockStockQuoteProvider.Mbank.Quotes)
//                {
//                    var newQuoteRow = inMemoryTable.NewRow();

//                    newQuoteRow[Constants.TickerName] = quote.Ticker;
//                    newQuoteRow[Constants.DateName] = quote.Date;
//                    newQuoteRow[Constants.OpenName] = quote.Open;
//                    newQuoteRow[Constants.HighName] = quote.High;
//                    newQuoteRow[Constants.LowName] = quote.Low;
//                    newQuoteRow[Constants.CloseName] = quote.Close;
//                    newQuoteRow[Constants.VolName] = quote.Volume;

//                    inMemoryTable.Rows.Add(newQuoteRow);
//                }
//                sqlConnection.Open();
//                sqlBulkCopy.WriteToServer(inMemoryTable);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                throw;
//            }
//            finally
//            {
//                sqlConnection?.Dispose();
//                inMemoryTable?.Dispose();
//                ((IDisposable)sqlBulkCopy)?.Dispose();
//            }
//        }

//        public IList<TEntity> GetAll()
//        {
//            throw new NotImplementedException();
//        }

//        public void Remove(TEntity entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveRange(IEnumerable<TEntity> entities)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
