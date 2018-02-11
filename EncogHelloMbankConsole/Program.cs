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
            const string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;MultipleActiveResultSets=True;";
            //const string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true";
            const string inputDirectory = @"C:\Users\John\Downloads\mstcgl";
            const int ommitStocksSmallerThan = 200;
            const int ommitDeadStocksDate = 20180209;
            const double errorThreshold = 0.02;
            const double errorDeltaThreshold = 0.0001;
            bool recreateDb = false;

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var logger = new AggregateLogger(new ConsoleLogger { InfoColor = ConsoleColor.Gray }, new FileLoggerBase("console"));
            var oneDataSet = GetBasicMlDataSet(connectionStr, recreateDb, logger, inputDirectory, ommitStocksSmallerThan, ommitDeadStocksDate);
            var network = SetupNetwork();
            //var train = new Backpropagation(network, trainingSet, 0.07, 0.07);
            var trainAlgorithm = new ResilientPropagation(network, oneDataSet);
            Train(logger, trainAlgorithm, errorDeltaThreshold, errorThreshold);

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

        static BasicNetwork SetupNetwork()
        {
            var basicNetwork = new BasicNetwork();
            basicNetwork.AddLayer(new BasicLayer(null, true, 5));
            basicNetwork.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 230));
            //network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 310));
            //network.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 230));
            basicNetwork.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            basicNetwork.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            basicNetwork.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            basicNetwork.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            basicNetwork.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 23));
            basicNetwork.AddLayer(new BasicLayer(new ActivationClippedLinear(), true, 5));
            basicNetwork.AddLayer(new BasicLayer(new ActivationClippedLinear(), false, 1));


            basicNetwork.Structure.FinalizeStructure();
            basicNetwork.Reset();
            return basicNetwork;
        }

        static void Train(ILogger logger, ResilientPropagation train, double errorDeltaThreshold,
            double errorThreshold)
        {
            var watch = Stopwatch.StartNew();
            var learningTime = default(TimeSpan);
            int? errorDelta;
            var errorsList = new List<double>();
            bool errorDeltaThresholdReached;
            bool errorThresholdAchieved;
            do
            {
                logger.LogInfo(@"Training... ");
                train.Iteration();
                errorsList.Add(train.Error);
                errorDelta = errorsList.Count > 1
                    ? (int?)(errorsList[train.IterationNumber - 1] - errorsList[train.IterationNumber - 2])
                    : (int?)null;
                errorDeltaThresholdReached = (!errorDelta.HasValue || errorDelta.Value > errorDeltaThreshold);
                errorThresholdAchieved = train.Error > errorThreshold;
                logger.LogInfo(
                    $@"iteration {train.IterationNumber} completed in {watch.ElapsedMilliseconds.AsTime()}; error = {
                            train.Error
                        }, error delta = {errorDelta?.ToString() ?? "?"}");
                learningTime += watch.Elapsed;
                watch.Restart();
            } while (!errorThresholdAchieved || !errorDeltaThresholdReached);

            logger.LogInfo(
                $@"Training finished after {learningTime.TotalMilliseconds.AsTime()}, at iteration {
                        train.IterationNumber
                    }; error = {train.Error}");
        }

        static BasicMLDataSet GetBasicMlDataSet(string connectionStr, bool recreateDb, ILogger logger, string inputDirectory, int ommitStocksSmallerThan, int ommitDeadStocksDate)
        {
            var context = new StockEfContext(connectionStr);
            var unitOfWork = new StockEfUnitOfWork(context);

            var stocksDeserialized = default(List<Company>);

            var watch = Stopwatch.StartNew();

            if (context.DbExists() && !recreateDb)
            {
                stocksDeserialized = unitOfWork.Stocks.GetAll().ToList();
                logger.LogInfo(
                    $@"Fetched {stocksDeserialized.Count} companies from Db in {watch.ElapsedMilliseconds.AsTime()}");
                watch.Restart();
            }
            else
            {
                if (context.DbExists())
                {
                    context.DropDbIfExists();

                    logger.LogInfo($@"Dropped Db in {watch.ElapsedMilliseconds.AsTime()}");
                    watch.Restart();
                }

                context.CreateDbIfNotExists();

                logger.LogInfo($@"Created Db in {watch.ElapsedMilliseconds.AsTime()}");
                watch.Restart();

                var directoryService = new IOService();
                var stocksRaw = directoryService.ReadDirectory(inputDirectory);

                logger.LogInfo($@"Read {stocksRaw.Count} in {watch.ElapsedMilliseconds.AsTime()} from {inputDirectory}");
                watch.Restart();

                stocksDeserialized = new StocksBulkDeserializer().Deserialize(stocksRaw);

                logger.LogInfo($@"Deserialized {stocksDeserialized.Count} in {watch.ElapsedMilliseconds.AsTime()}");
                watch.Restart();

                var bulkInserter = new CompanyBulkInserter(connectionStr);
                bulkInserter.BulkInsert(stocksDeserialized);

                logger.LogInfo($@"Saved {stocksDeserialized.Count} to {connectionStr} in {watch.ElapsedMilliseconds.AsTime()}");
                watch.Restart();
            }

            var normalizer = new StockQuotesToNormalizedMatrix();

            var allStocksNormalized = new List<BasicMLDataSet>();
            var matrixConverter = new MatrixToMLData();
            Parallel.ForEach(stocksDeserialized, (stock) =>
            {
                if (stock.Quotes.Count >= ommitStocksSmallerThan)
                    allStocksNormalized.Add(matrixConverter.ConvertToHighPred(normalizer.Convert(stock.Quotes.ToList())));
            });

            logger.LogInfo(
                $@"Converted and normalized {allStocksNormalized.Count} in {watch.ElapsedMilliseconds.AsTime()}. Ommited {
                        stocksDeserialized.Count - allStocksNormalized.Count
                    }. Reson: less than {ommitStocksSmallerThan} samples.");
            watch.Restart();

            var oneDataSet = new BasicMLDataSet();
            foreach (var stockNormal in allStocksNormalized)
            {
                foreach (var mlDataPair in stockNormal.Data)
                {
                    oneDataSet.Add(mlDataPair);
                }
            }

            logger.LogInfo($@"Constructed dataset with {oneDataSet.Count} samples in {watch.ElapsedMilliseconds.AsTime()}");
            watch.Restart();
            return oneDataSet;
        }
    }
}
