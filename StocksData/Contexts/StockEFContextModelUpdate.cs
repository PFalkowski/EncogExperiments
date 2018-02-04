using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksData.Contexts
{
    public class StockEFContextModelUpdate : StockEFContext
    {
        public StockEFContextModelUpdate(string connectionStr) : base(connectionStr)
        {
            Database.SetInitializer<StockEFContext>(new DropCreateDatabaseIfModelChanges<StockEFContext>());
        }
    }
}
