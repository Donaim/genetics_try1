using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VNNAddOn;
using VNNLib;

namespace core
{
    public abstract class Agent : NatureObject
    {
        public readonly Agent Parent;
        public readonly Genome G;

        protected readonly ActionsBase[] actions;
        protected abstract IEnumerable<ActionsBase> getActions();
        
        protected Agent(Field f, Agent parent) : base(f)
        {
            actions = getActions().ToArray();
            OnDie += Dispose;

            Parent = parent;
            G = Parent.OnBorn(this);
        }
        protected Agent(Genome g)
        {
            Parent = null;
            G = g;
        }

        public abstract double GetFitness();

        public void Do()
        {
            var ps = new List<ActionsBase.PossibilityResult>();

            for (int i = 0, n = actions.Length; i < n; i++) { actions[i].GetPossibilities(ps); }

            for (int i = 0, n = ps.Count; i < n; i++) { ps[i].Score = Evaluate(ps[i].Position); }

            //var max = ActionsBase.Possibility.HoldPossibility(this);
            var max = ActionsBase.PossibilityResult.Default(this);
            U.RShuffle(ps);
            for (int i = 0, n = ps.Count; i < n; i++)
            {
                if (ps[i].Score > max.Score) { max = ps[i]; }
            }

            //if(max.Realize.Method.Name == "eat") { Console.WriteLine($"I ({px},{py}) ate it.."); }

            if(max.Score > 0)
            {
                //if(Math.Abs(max.Score - 0.5) > 0.1) { max = ps[U.Rand(ps.Count)]; }

                max.Job();
                max.Uncertain();
            }
        }
        public virtual double Evaluate(double[] inputs) => G.Evaluate(inputs);
        public virtual void PreDo() { }

        public bool Dead { get; private set; } = false;
        public void Die()
        {
            Dead = true;
            //G.Decrease();

            OnDie();
        }
        public event Action OnDie;
        public event Func<Agent, Genome> OnBorn;
        protected void stdOnDie() => Dispose();

        public abstract void Introspect(U.DoubleStream stream);
        public abstract void SerializeSurrounding(U.DoubleStream stream);
    }
}
