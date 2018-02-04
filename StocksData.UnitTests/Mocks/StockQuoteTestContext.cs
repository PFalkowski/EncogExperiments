using System.Data;
using System.Data.Entity;
using StocksData.Contexts;
using StocksData.Models;

namespace StocksData.UnitTests.Mocks
{
    public class StockQuoteTestContext : DbContext
    {
        public StockQuoteTestContext(IDbConnection connection) : base(connection.ConnectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<StockEFContext>());
        }
        public virtual DbSet<StockQuote> StockQuotes { get; set; }
    }
}
