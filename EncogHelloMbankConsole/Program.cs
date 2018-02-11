using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Extensions.Standard;
using LoggerLite;
using StocksData.Adapters;
using StocksData.Contexts;
using StocksData.Mappings;
using StocksData.Model;
using StocksData.Services;
using StocksData.UnitsOfWork;
using System.Data.SqlClient;

namespace EncogHelloMbankConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionStr = @"server=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;";
            const string inputDirectory = @"C:\Users\John\Downloads\mstcgl";
            const int ommitStocksSmallerThan = 200;
            bool TryLoadingFromDb = true;

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var logger = new AggregateLogger(new ConsoleLogger { InfoColor = ConsoleColor.Gray }, new FileLoggerBase("console"));
            var timer = Stopwatch.StartNew();


            var context = new StockEfContext(connectionStr);
            var unitOfWork = new StockEfUnitOfWork(context);

            var stocksDeserialized = default(List<Company>);

            if (context.DbExists())
            {
                if (TryLoadingFromDb)
                {
                    stocksDeserialized = unitOfWork.Stocks.GetAll().ToList();
                    logger.LogInfo($@"Fetched {stocksDeserialized} companies from Db in {timer.ElapsedMilliseconds.AsTime()}");
                    timer.Restart();
                }
                else
                {
                    context.DropDb();

                    logger.LogInfo($@"Dropped Db in {timer.ElapsedMilliseconds.AsTime()}");
                    timer.Restart();
                }
            }

            if (!context.DbExists())
            {
                context.CreateDbIfNotExists();

                logger.LogInfo($@"Created Db in {timer.ElapsedMilliseconds.AsTime()}");
                timer.Restart();

                var directoryService = new IOService();
                var stocksRaw = directoryService.ReadDirectory(inputDirectory);

                logger.LogInfo($@"Read {stocksRaw.Count} in {timer.ElapsedMilliseconds.AsTime()} from {inputDirectory}");
                timer.Restart();

                stocksDeserialized = new StocksBulkDeserializer().Deserialize(stocksRaw);

                logger.LogInfo($@"Deserialized {stocksDeserialized.Count} in {timer.ElapsedMilliseconds.AsTime()}");
                timer.Restart();

                var bulkInserter = new CompanyBulkInserter(connectionStr);
                bulkInserter.BulkInsert(stocksDeserialized);

                logger.LogInfo($@"Saved {stocksDeserialized.Count} to {connectionStr} in {timer.ElapsedMilliseconds.AsTime()}");
                timer.Restart();
            }



            //logger.LogInfo($@"Initiated db connection in {timer.ElapsedMilliseconds.AsTime()}");
            //timer.Restart();


            var normalizer = new StockQuotesToNormalizedMatrix();

            var allStocksNormalized = new List<BasicMLDataSet>();
            var matrixConverter = new MatrixToMLData();
            Parallel.ForEach(stocksDeserialized, (stock) =>
            {
                if (stock.Quotes.Count >= ommitStocksSmallerThan)
                    allStocksNormalized.Add(matrixConverter.ConvertToHighPred(normalizer.Convert(stock.Quotes.ToList())));
            });

            logger.LogInfo($@"Converted and normalized {allStocksNormalized.Count} in {timer.ElapsedMilliseconds.AsTime()}. Ommited {stocksDeserialized.Count - allStocksNormalized.Count}. Reson: less than {ommitStocksSmallerThan} samples.");
            timer.Restart();

            var oneDataSet = new BasicMLDataSet();
            foreach (var stockNormal in allStocksNormalized)
            {
                foreach (var mlDataPair in stockNormal.Data)
                {
                    oneDataSet.Add(mlDataPair);
                }
            }
            logger.LogInfo($@"Constructed dataset with {oneDataSet.Count} samples in {timer.ElapsedMilliseconds.AsTime()}");

            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, 5));
            network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 230));
            //network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 310));
            //network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 230));
            network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 5));
            network.AddLayer(new BasicLayer(new ActivationClippedLinear(), false, 1));


            network.Structure.FinalizeStructure();
            network.Reset();

            //var train = new Backpropagation(network, trainingSet, 0.07, 0.07);
            var train = new ResilientPropagation(network, oneDataSet);

            var learningTime = default(TimeSpan);
            do
            {
                logger.LogInfo(@"Training... ");
                train.Iteration();
                logger.LogInfo($@"iteration {train.IterationNumber} completed in {timer.ElapsedMilliseconds.AsTime()}; error = {train.Error}");
                learningTime += timer.Elapsed;
                timer.Restart();
            } while (train.Error > 0.02);
            logger.LogInfo($@"Training finished after {learningTime.TotalMilliseconds.AsTime()}, at iteration {train.IterationNumber}; error = {train.Error}");

            var testSet = oneDataSet.Skip(1200).ToList().Take(200).ToList();
            var avgError = 0.0;
            foreach (var testSample in testSet)
            {
                var output = network.Compute(testSample.Input);
                var difference = testSample.Ideal[0] - output[0];
                avgError += Math.Abs(difference);
            }

            logger.LogInfo($@"evaluation of training data resulted in avgError = {avgError}");
            Console.Read();
        }
    }
}
