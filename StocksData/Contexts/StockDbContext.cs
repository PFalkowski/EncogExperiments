using System.Data;
using System.Data.Entity;
using StocksData.Models;

namespace StocksData.Contexts
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(string connectionStr) : base(connectionStr)
        {
            Database.SetInitializer<StockDbContext>(new CreateDatabaseIfNotExists<StockDbContext>());
        }
        public virtual DbSet<Company> StockCompaniesQuotes { get; set; }
    }
}
