using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using core;
using VNNAddOn;
using VNNLib;

namespace stdSimpleNeural
{
    public abstract class NeuralGenome : Genome
    {
        public readonly vnn NN;
        public readonly trainerModern TR;

        protected NeuralGenome() : this(createNewNN()) { }
        protected NeuralGenome(vnn nn)
        {
            NN = nn;
            TR = new trainerModern(NN);
        }

        const int NNHIDEN = 30;
        public static vnn createNewNN()
        {
            var nn = new vnn(U.InputSize, NNHIDEN, 1);
            nn.RandomizeUniform(mult1: 4.5);
            return nn;
        }

        public override double Evaluate(double[] inputs)
        {
            NN.feedForward(inputs);

            //Console.Write("Hidden\t: ");
            //StdTest.WeightsTest.PrintHist(NN.hiddenNeurons);
            //Console.WriteLine($"OUT: {NN.outputNeurons[0].ToString("N2")}");

            return NN.outputNeurons[0];
        }
    }
    public interface ITrackHistory
    {
        U.ModuloList<EvaluateUnit> History { get; }
        void CleanHistory();
    }
    public struct EvaluateUnit
    {
        public readonly double[] Position;
        public readonly double Value;

        public EvaluateUnit(double[] inputs, double value)
        {
            Position = new double[inputs.Length];
            Array.Copy(inputs, Position, inputs.Length);

            Value = value;
        }
    }
}
