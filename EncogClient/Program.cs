using System;
using System.Diagnostics;
using Encog.Engine.Network.Activation;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.NeuralData;

namespace EncogClient
{
    public class Program
    {
        private static readonly double[][] XorInput ={
            new double[2] { 0.0, 0.0 },
            new double[2] { 1.0, 0.0 },
            new double[2] { 0.0, 1.0 },
            new double[2] { 1.0, 1.0 } };

        private static readonly double[][] XorIdeal = {
            new double[1] { 0.0 },
            new double[1] { 1.0 },
            new double[1] { 1.0 },
            new double[1] { 0.0 } };

        public static void Main()
        {
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 2));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 6));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 1));
            network.Structure.FinalizeStructure();
            network.Reset();

            INeuralDataSet trainingSet = new BasicNeuralDataSet(XorInput, XorIdeal);

            ITrain train = new ResilientPropagation(network, trainingSet);

            int epoch = 1;
            var timer = Stopwatch.StartNew();
            do
            {
                train.Iteration();
                epoch++;
            } while ((epoch < 50000) && (train.Error > 0.0001));

            timer.Stop();

            Console.WriteLine("Neural Network Results:");
            foreach (var pair in trainingSet)
            {
                var output = network.Compute(pair.Input);
                Console.WriteLine(pair.Input[0] + "," + pair.Input[1]
                        + ", actual=" + output[0] + ", ideal=" + pair.Ideal[0]);
            }
            Console.WriteLine($"Completed {epoch} epochs in {timer.Elapsed} ({(float)timer.ElapsedMilliseconds / epoch} ms per epoch)");
            Console.ReadLine();
        }
    }
}
