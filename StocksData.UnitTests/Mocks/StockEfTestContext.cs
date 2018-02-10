using System.Data.Entity;
using StocksData.Contexts;

namespace StocksData.UnitTests.Mocks
{
    public class StockEfTestContext : StockEfContext
    {
        public StockEfTestContext(string connectionStr) : base(connectionStr)
        {
            Database.SetInitializer<StockEfTestContext>(new CreateDatabaseIfNotExists<StockEfTestContext>());
        }
    }
}
