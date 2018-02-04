using System.Data;
using System.Data.Entity;
using StocksData.Contexts;
using StocksData.Model;

namespace StocksData.UnitTests.Mocks
{
    public class StockQuoteTestContext : DbContext
    {
        public StockQuoteTestContext(string connection) : base(connection)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<StockEfContext>());
        }
        public virtual DbSet<StockQuote> StockQuotes { get; set; }
    }
}
