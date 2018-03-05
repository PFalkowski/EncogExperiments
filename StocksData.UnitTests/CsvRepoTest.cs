using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StocksData.Contexts;
using StocksData.Mappings;
using StocksData.Model;
using StocksData.Services;
using StocksData.UnitsOfWork;
using Xunit;

namespace StocksData.UnitTests
{
    public class CsvRepoTest
    {
        [Fact]
        public void SavingToCsvRepoWorks()
        {
            var deserializer = new StocksDeserializer(new StockQuoteCsvClassMap());
            var allStocks = new List<Company> {
                deserializer.Deserialize(Encoding.UTF8.GetString(Properties.Resources._11BIT)),
                deserializer.Deserialize(Encoding.UTF8.GetString(Properties.Resources.CDPROJEKT)),
                deserializer.Deserialize(Encoding.UTF8.GetString(Properties.Resources.MBANK)) };

            var outputFile = new FileInfo(Path.ChangeExtension(nameof(SavingToCsvRepoWorks), "csv"));
            using (var unitOfWork = new StockCsvUnitOfWork(new StockCsvContext<Company>(outputFile)))
            {
                foreach (var stock in allStocks)
                {
                    unitOfWork.Repository.AddOrUpdate(stock);
                    unitOfWork.Complete();
                }
            }

        }
        [Fact]
        public void GetSpecificRecordFromCsvRepo()
        {
            var deserializer = new StocksDeserializer(new StockQuoteCsvClassMap());
            var allStocks = new List<Company> {
                deserializer.Deserialize(Encoding.UTF8.GetString(Properties.Resources._11BIT)),
                deserializer.Deserialize(Encoding.UTF8.GetString(Properties.Resources.CDPROJEKT)),
                deserializer.Deserialize(Encoding.UTF8.GetString(Properties.Resources.MBANK)) };

            var outputFile = new FileInfo(Path.ChangeExtension(nameof(GetSpecificRecordFromCsvRepo), "csv"));
            using (var unitOfWork = new StockCsvUnitOfWork(new StockCsvContext<Company>(outputFile)))
            {
                unitOfWork.Repository.AddRange(allStocks);

                var oneStock = unitOfWork.Repository.Entities.FirstOrDefault(x => x.Ticker == "MBANK");
                unitOfWork.Repository.Remove(oneStock);

                unitOfWork.Complete();
            }
        }
        //[Fact]
        //public void DeleteAllStocksFromCsvRepo()
        //{
        //    var outputFile = new FileInfo("test23443.txt");
        //    var factory = new StockQuotesCsvUnitOfWorkFactory(outputFile);
        //    using (var unitOfWork = factory.GetInstance())
        //    {
        //        unitOfWork.StockRepository.RemoveRange();
        //        unitOfWork.Complete();
        //    }
        //}
        //[Fact]
        //public void RemoveOneStockFromCsvRepo()
        //{
        //    var outputFile = new FileInfo("test23443.txt");
        //    var factory = new StockQuotesCsvUnitOfWorkFactory(outputFile);
        //    using (var unitOfWork = factory.GetInstance())
        //    {
        //        unitOfWork.StockRepository.Remove(unitOfWork.StockRepository.Get());
        //        unitOfWork.Complete();
        //    }
        //}
    }
}
