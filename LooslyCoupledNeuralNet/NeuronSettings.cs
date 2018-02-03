using System;
using System.Collections.Generic;
using System.Text;

namespace LooslyCoupledNeuralNet
{
    public class NeuronSettings
    {
        public double InitialLearningRate { get; set; }
        public Func<double, double> LearningRateDecayFunc { get; set; }
        public Func<double, double> ActivationFunc { get; set; }
    }
}
