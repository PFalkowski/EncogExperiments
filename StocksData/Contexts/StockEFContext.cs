using System.Data.Entity;
using StocksData.Model;

namespace StocksData.Contexts
{
    public class StockEfContext : DbContext
    {
        public StockEfContext(string connectionStr) : base(connectionStr)
        {
            Database.SetInitializer<StockEfContext>(new CreateDatabaseIfNotExists<StockEfContext>());
        }
        public virtual DbSet<Company> StockCompaniesQuotes { get; set; }
    }
}
