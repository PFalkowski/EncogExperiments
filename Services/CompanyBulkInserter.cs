using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StocksData.Model;

namespace Services
{
    public class CompanyBulkInserter : BulkInserter<Company>
    {
        public BulkInserter<StockQuote> StockQuoteBulkInserter { get; set; }
        public CompanyBulkInserter(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public override void BulkInsert(IEnumerable<Company> companies)
        {
            var bulkInserter = StockQuoteBulkInserter ?? new StockQuotesBulkInserter(ConnectionString);

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                foreach (var company in companies)
                {
                    using (var command = new SqlCommand("insert into [Companies] (Ticker) values (@value)", connection))
                    {
                        command.Parameters.Add("@value", SqlDbType.VarChar);
                        command.Parameters["@value"].Value = company.Ticker;
                        command.ExecuteNonQuery();
                    }
                    bulkInserter.BulkInsert(company.Quotes);
                }
            }
        }
    }
}
