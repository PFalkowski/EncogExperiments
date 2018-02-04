using System.Data;
using System.Data.Entity;
using StocksData.Models;

namespace StocksData.Contexts
{
    public class StockEFContext : DbContext
    {
        public StockEFContext(string connectionStr) : base(connectionStr)
        {
            Database.SetInitializer<StockEFContext>(new CreateDatabaseIfNotExists<StockEFContext>());
        }
    }
}
