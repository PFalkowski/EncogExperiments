using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksData.Contexts
{
    public class StockDbContextModelUpdate : StockDbContext
    {
        public StockDbContextModelUpdate(string connectionStr) : base(connectionStr)
        {
            Database.SetInitializer<StockDbContext>(new DropCreateDatabaseIfModelChanges<StockDbContext>());
        }
    }
}
