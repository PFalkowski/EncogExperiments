using System.Data.Entity;
using StocksData.Model;

namespace StocksData.Contexts
{
    public class StockEfContext : DbContext
    {
        public bool DbExists() => base.Database.Exists();
        public void CreateDbIfNotExists() => base.Database.CreateIfNotExists();
        public void DropDbIfExists() => base.Database.Delete();

        public StockEfContext(string connectionStr) : base(connectionStr)
        {
            Database.SetInitializer<StockEfContext>(new CreateDatabaseIfNotExists<StockEfContext>());
        }
        public virtual DbSet<Company> StockCompaniesQuotes { get; set; }
    }
}
