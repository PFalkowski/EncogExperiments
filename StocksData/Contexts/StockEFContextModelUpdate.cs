using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksData.Contexts
{
    public class StockEfContextModelUpdate : StockEfContext
    {
        public StockEfContextModelUpdate(string connectionStr) : base(connectionStr)
        {
            Database.SetInitializer<StockEfContext>(new DropCreateDatabaseIfModelChanges<StockEfContext>());
        }
    }
}
