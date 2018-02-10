using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksData.Contexts
{
    public class StockEfMigrationContext : StockEfContext
    {
        public StockEfMigrationContext() : base(Properties.Settings.Default.MigrationConnectionStr)
        {
        }
    }
}
