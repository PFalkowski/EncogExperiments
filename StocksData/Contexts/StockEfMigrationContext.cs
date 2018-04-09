namespace StocksData.Contexts
{
    public class StockEfMigrationContext : StockEfContext
    {
        public StockEfMigrationContext() : base(Properties.Settings.Default.MigrationConnectionStr)
        {
        }
    }
}
