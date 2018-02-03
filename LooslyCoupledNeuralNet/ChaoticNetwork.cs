using LooslyCoupledNeuralNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace LooslyCoupledNeuralNet
{
    public class ChaoticNetwork
    {
        public Random Rng { get; set; }
        public NeuronSettings Settings { get; private set; }
        public List<InputNeuron> InputNeurons { get; set; } = new List<InputNeuron>();
        public List<INeuron> AllNeurons { get; set; } = new List<INeuron>();
        public ChaoticNetwork(Random rng, NeuronSettings settings)
        {
            Rng = rng;
            Settings = settings;
        }

        public void RandomizeNewNetwork(int problemSize, int minNeurons, int maxNeurons, int minLayers, int maxLayers)
        {
            if (problemSize < 1) throw  new ArgumentException(nameof(problemSize));
            if (minNeurons < 1 || minNeurons < problemSize) throw new ArgumentException(nameof(minNeurons));
            if (maxNeurons < 1) throw new ArgumentException(nameof(maxNeurons));

            var noNeurons = Rng.Next(minNeurons, maxNeurons);
            var noLayers = Rng.Next(minLayers, maxLayers);
            var neuronsStack = new Stack<Neuron>();
            for (var i = 0; i < problemSize; ++i)
            {
                var newInputNeuron = new InputNeuron();
                InputNeurons.Add(newInputNeuron);
                var toBeConnected = neuronsStack.Pop();

            }
            for (var i = 0; i < noNeurons; ++i)
            {
                var newNeuron = new Neuron(Settings);
                AllNeurons.Add(newNeuron);
                neuronsStack.Push(newNeuron);
            }
        }
    }
}
