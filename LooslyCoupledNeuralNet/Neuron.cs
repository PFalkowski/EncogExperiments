using System;
using System.Collections.Generic;

namespace LooslyCoupledNeuralNet
{
    public class Neuron : INeuron
    {
        public double LearningRate { get; set; } = 0.01;
        public Func<double, double> LrateDecayFunc { get; }
        public Func<double, double> ActivationFunc { get; }
        public const double Threshold = .7;
        public Dictionary<INeuron, double> PrevInputNeuronsSignals { get; private set; }
        public Dictionary<INeuron, double> InputNeuronsSignals { get; } = new Dictionary<INeuron, double>();
        public Dictionary<INeuron, double> OutputNeuronsWeights { get; } = new Dictionary<INeuron, double>();
        public double SynapticPotential { get; set; }
        public double OutputPotential => Response();

        public Neuron(NeuronSettings settings)
        {
            LrateDecayFunc = settings.LearningRateDecayFunc;
            ActivationFunc = settings.ActivationFunc;
            LearningRate = settings.InitialLearningRate;
        }

        public double Response()
        {
            return ActivationFunc(
                OutputNeuronsWeights.Count > 0 ? SynapticPotential / OutputNeuronsWeights.Count : SynapticPotential);
        }


        public void Reward(double value, Neuron outputNeuron)
        {
            OutputNeuronsWeights[outputNeuron] += LearningRate * value;
            var rewardScaled = PrevInputNeuronsSignals.Count > 0 ? value / PrevInputNeuronsSignals.Count : value;
            foreach (var prevInputNeuronsSignal in
                PrevInputNeuronsSignals)
            {
                prevInputNeuronsSignal.Key.Reward(rewardScaled, this);
            }
        }

        public void Activate(INeuron you)
        {
            SynapticPotential += you.OutputNeuronsWeights[this] * you.OutputPotential;
            if (InputNeuronsSignals.ContainsKey(you))
            {
                InputNeuronsSignals[you] += you.OutputPotential;
            }
            else
            {
                InputNeuronsSignals.Add(you, you.OutputPotential);
            }
            if (ActivationFunc(SynapticPotential) >= Threshold)
            {
                foreach (var outputNeuron in OutputNeuronsWeights.Keys)
                {
                    outputNeuron.Activate(this);
                }

                Refract();
            }
        }

        public void Refract()
        {
            SynapticPotential = 0;
            PrevInputNeuronsSignals.Clear();
            PrevInputNeuronsSignals = InputNeuronsSignals;
            InputNeuronsSignals.Clear();
            LearningRate = LrateDecayFunc(LearningRate);
        }

        public void ConnectInput(INeuron neuron)
        {
            InputNeuronsSignals.Add(neuron, default(double));
        }
    }
}
