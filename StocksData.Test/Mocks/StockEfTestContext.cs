using System.Data.Entity;
using StocksData.Contexts;

namespace StocksData.Test.Mocks
{
    public class StockEfTestContext : StockEfContext
    {
        public StockEfTestContext(string connectionStr) : base(connectionStr)
        {
            Database.SetInitializer<StockEfTestContext>(new DropCreateDatabaseAlways<StockEfTestContext>());
        }
    }
}
