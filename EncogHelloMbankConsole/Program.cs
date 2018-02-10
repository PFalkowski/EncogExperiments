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

namespace EncogHelloMbankConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new AggregateLogger(new ConsoleLogger { InfoColor = ConsoleColor.Gray}, new FileLoggerBase("console"));

            var ommitStocksLessThanDays = 200;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var timer = Stopwatch.StartNew();

            const string connectionStr = @"server=(localdb)\MSSQLLocalDB;Initial Catalog=StockMarketDb;Integrated Security=True;";
            var unitOfWork = new StockEfUnitOfWork(new StockEfContext(connectionStr));

            //var allFiles = new IOService().ReadDirectory("C:\\Users\\John\\Downloads\\mstcgl", "*.mst");
            //logger.LogInfo($@"Read {allFiles.Count} files in {timer.ElapsedMilliseconds.AsTime()}");

            logger.LogInfo($@"Initiated db connection in {timer.ElapsedMilliseconds.AsTime()}");

            timer.Restart();

            var directoryService = new IOService();
            var stocksRaw = directoryService.ReadDirectory(@"C:\Users\John\Downloads\mstcgl");

            logger.LogInfo($@"Read {stocksRaw.Count} in {timer.ElapsedMilliseconds.AsTime()}");

            var stocksDeserialized = new StocksBulkDeserializer().Deserialize(stocksRaw);

            logger.LogInfo($@"Deserialized {stocksDeserialized.Count} in {timer.ElapsedMilliseconds.AsTime()}");

            timer.Restart();
            var normalizer = new StockQuotesToNormalizedMatrix();

            var allStocksNormalized = new List<BasicMLDataSet>();
            var matrixConverter = new MatrixToMLData();
            Parallel.ForEach(stocksDeserialized, (stock) =>
            {
                if (stock.Quotes.Count >= ommitStocksLessThanDays)
                    allStocksNormalized.Add(matrixConverter.ConvertToHighPred(normalizer.Convert(stock.Quotes.ToList())));
            });

            logger.LogInfo($@"Converted and normalized {allStocksNormalized.Count} in {timer.ElapsedMilliseconds.AsTime()}. Ommited {stocksDeserialized.Count - allStocksNormalized.Count}. Reson: less than {ommitStocksLessThanDays} samples.");

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
