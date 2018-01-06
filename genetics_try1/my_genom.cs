using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using core;
using VNNAddOn;
using VNNLib;
using stdSimpleNeural;

namespace genetics_try1
{
    public class MyGenome : NeuralGenome
    {
        public MyGenome() { }

        public override void OnDies(LearnManager lm, Agent baseOfMutation)
        {
            var tracker = (ITrackHistory)baseOfMutation;
            double fit = baseOfMutation.GetFitness();

            var list = tracker.History.Buffer;
            for(int i = 0, n = tracker.History.Length; i < n; i++)
            {
                var correct = LearnManager.GetCorrect(list[i].Value, lm.GetMeanRatioNormalized(fit));
                TR.TrainOne(list[i].Position, new double[] { correct }, learningRate: 0.1, momentum: 0);
            }

            tracker.CleanHistory();
        }

        public const double MUTATE_RATE = 0.05;
        public override Genome OnBirth(LearnManager lm, Agent parent)
        {
            Genome re;
            //var lma = (MyLearnManager)lm;

            if (U.Rand() > MUTATE_RATE) { re = lm.GetWagedRandomGenome(); }
            else { re = new MyGenome(); }

            return re;
        }
        public override string ToString() => 'x' + base.ToString();
    }
    public class MyGenericGenome : NeuralGenome
    {
        public MyGenericGenome() { }
        public MyGenericGenome(vnn nn) : base(nn) { }
        
        public override void OnDies(LearnManager lm, Agent baseOfMutation) { /*EMPTY*/ }

        public const double MUTATE_RATE = 0.2;
        public override Genome OnBirth(LearnManager lm, Agent parent)
        {
            //var lma = (MyLearnManager)lm;
            Genome re;
            if(U.Rand() > MUTATE_RATE) { re = lm.GetWagedRandomGenome(); }
            else { re = mutateSome((MyGenericGenome)parent.G); }

            return re;
        }
        MyGenericGenome mutateSome(MyGenericGenome original)
        {
            var re = original.NN.Copy();

            re.wInputHidden[U.Rand(original.NN.nInput + 1), U.Rand(original.NN.nHidden)] = U.Rand() * 6;
            re.wHiddenOutput[U.Rand(original.NN.nHidden + 1), U.Rand(original.NN.nOutput)] = U.Rand() * 6;

            return new MyGenericGenome(re);
        }

        public override string ToString() => 'i' + base.ToString();
    }
    public class MyNeuralishGenome : NeuralGenome
    {
        private MyNeuralishGenome(vnn dna) : base(dna) { }
        public MyNeuralishGenome() { }

        public override void OnDies(LearnManager lm, Agent baseOfMutation) { /*EMPTY*/ }

        public const double MUTATE_RATE = 0.1;
        public override Genome OnBirth(LearnManager lm, Agent parent)
        {
            //var lma = (MyLearnManager)lm;
            Genome re;
            if (U.Rand() > MUTATE_RATE)
            {
                re = lm.GetWagedRandomGenome();

                var nn = ((MyNeuralishGenome)re).NN.Copy();
                nn.wInputHidden[U.Rand(nn.nInput + 1), U.Rand(nn.nHidden)] = U.Rand() * 6;
                nn.wHiddenOutput[U.Rand(nn.nHidden + 1), U.Rand(nn.nOutput)] = U.Rand() * 6;

                return new MyNeuralishGenome(nn);
            }
            else
            {
                return mutateSome(lm, parent);
            }
        }

        MyNeuralishGenome mutateSome(LearnManager lm, Agent parent)
        {
            return new MyNeuralishGenome();
            //var nn = ((MyNeuralishGenome)parent.G).NN.Copy();

            ////return;
            //var fit = parent.GetFitness();

            //var tr = new trainerModern(nn);
            ////var tr = ((MyGenome)G).TR;
            //foreach (var eu in parent.History)
            //{
            //    var correct = LearnManager.GetCorrect(eu.Value, lm.GetMeanRatioNormalized(fit));
            //    tr.TrainOne(eu.Position, new double[] { correct }, learningRate: 0.1, momentum: 0);
            //}

            //return new MyNeuralishGenome(nn);
        }

        public override string ToString() => 'z' + base.ToString();
    }
}
