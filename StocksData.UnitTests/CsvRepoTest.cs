using System.IO;
using System.Linq;
using StocksData.Contexts;
using StocksData.Model;
using StocksData.Services;
using StocksData.UnitsOfWork;
using Xunit;

namespace StocksData.UnitTests
{
    public class CsvRepoTest
    {
        [Fact]
        public void ReadAllFilesFromDirAndSaveToCsvRepo()
        {
            var allStocks  = new StocksFileProvider().ReadStocksFrom(@"C:\Users\John\Downloads\mstcgl", "*.mst");

            var outputFile = new FileInfo(Path.ChangeExtension(nameof(ReadAllFilesFromDirAndSaveToCsvRepo), "csv"));
            using (var unitOfWork = new StockCsvFUnitOfWork(new StockCsvContext<Company>(outputFile)))
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
            var allStocks = new StocksFileProvider().ReadStocksFrom(@"C:\Users\John\Downloads\mstcgl", "*.mst");

            var outputFile = new FileInfo(Path.ChangeExtension(nameof(GetSpecificRecordFromCsvRepo), "csv"));
            using (var unitOfWork = new StockCsvFUnitOfWork(new StockCsvContext<Company>(outputFile)))
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
