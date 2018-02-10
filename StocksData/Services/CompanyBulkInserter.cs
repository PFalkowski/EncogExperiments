using StocksData.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksData.Services
{
    public class CompanyBulkInserter : IBulkInserter<Company>
    {
        public IBulkInserter<StockQuote> StockQuoteBulkInserter { get; set; }
        public CompanyBulkInserter(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public override void BulkInsert(IEnumerable<Company> companies)
        {
            var bulkInserter = StockQuoteBulkInserter ?? new StockQuotesBulkInserter(ConnectionString);

            using (var connection = new SqlConnection(ConnectionString))
            {
                foreach (var company in companies)
                {
                    using (var command = new SqlCommand("insert into [Companies] (Ticker) values (@value)", connection))
                    {
                        command.Parameters.Add("@value", SqlDbType.VarChar);
                        command.Parameters["@value"].Value = company.Ticker;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    bulkInserter.BulkInsert(company.Quotes);
                }
            }
        }
    }
}
