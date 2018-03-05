using CsvHelper.Configuration;
using StocksData.Model;

namespace StocksData.Mappings
{
    public sealed class StockQuoteCsvClassMap : ClassMap<StockQuote>
    {
        public StockQuoteCsvClassMap()
        {
            Map(m => m.Ticker).Name("<TICKER>");
            Map(m => m.Date).Name("<DTYYYYMMDD>");
            Map(m => m.Open).Name("<OPEN>");
            Map(m => m.High).Name("<HIGH>");
            Map(m => m.Low).Name("<LOW>");
            Map(m => m.Close).Name("<CLOSE>");
            Map(m => m.Volume).Name("<VOL>");
            Map(m => m.DateParsed).Ignore();
        }
    }
}
