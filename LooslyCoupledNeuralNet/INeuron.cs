using System.Collections.Generic;

namespace LooslyCoupledNeuralNet
{
    public interface INeuron
    {
        Dictionary<INeuron, double> OutputNeuronsWeights { get; }
        double OutputPotential { get; }
        double Response();
        void Reward(double value, Neuron outputNeuron);
        void Activate(INeuron you);
        double SynapticPotential { get; set; }
    }
}