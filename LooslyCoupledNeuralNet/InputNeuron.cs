using System;
using System.Collections.Generic;
using System.Text;

namespace LooslyCoupledNeuralNet
{
    public class InputNeuron : INeuron
    {
        public Dictionary<INeuron, double> OutputNeuronsWeights { get; set; }
        public double LearningRate { get; set; } = 0.01;

        public double OutputPotential => Response();

        public double SynapticPotential { get; set; }

        public void Activate(INeuron you)
        {
            throw new NotImplementedException();
        }

        public double Response()
        {
            return SynapticPotential;
        }

        public void Reward(double value, Neuron outputNeuron)
        {
            OutputNeuronsWeights[outputNeuron] += LearningRate * value;
        }
    }
}
