using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using core;
using VNNAddOn;
using VNNLib;

namespace genetics_try1
{
    public class MyLearnManager : LearnManager
    {
        //public readonly Dictionary<Genome, int> populations = new Dictionary<Genome, int>();

        public MyLearnManager(int list_size = 200) : base(list_size)
        {

        }

        protected override void onDies(Agent a)
        {
            a.G.OnDies(this, a);

            //if(populations[a.G] <= 1) { populations.Remove(a.G); }
            //else { populations[a.G]--; }
        }

        protected override Genome onBorn(Agent child)
        {
            Genome g = child.Parent.G.OnBirth(this, child.Parent);

            //if (populations.TryGetValue(g, out int p)) { populations[g]++; }
            //else { populations.Add(g, 1); }

            return g;
        }

        protected override Agent createNew(Field enviroment, Type genotype, Type prototype)
        {
            Genome genome = (Genome)Activator.CreateInstance(genotype, new object[] { });
            var adam = new AdamAgent(genome);

            //populations.Add(genome, 1);
            
            return (Agent)Activator.CreateInstance(prototype, new object[] { enviroment, adam });
        }
        class AdamAgent : Agent
        {
            public AdamAgent(Genome g) : base(g)
            {
                OnBorn += AdamAgent_TransfereGenome;
            }
            Genome AdamAgent_TransfereGenome(Agent arg) => G;

            public override int GetID => -1;

            public override double GetFitness()
            {
                throw new NotImplementedException();
            }

            public override void Introspect(U.DoubleStream stream)
            {
                throw new NotImplementedException();
            }

            public override void SerializeSurrounding(U.DoubleStream stream)
            {
                throw new NotImplementedException();
            }

            protected override IEnumerable<ActionsBase> getActions()
            {
                throw new NotImplementedException();
            }
        }
    }
}
