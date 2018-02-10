using System.Data.Entity;

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
