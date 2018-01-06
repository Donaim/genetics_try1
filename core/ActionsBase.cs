using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public abstract class ActionsBase
    {
        public readonly Agent Parent;
        public ActionsBase(Agent parent)
        {
            Parent = parent;
            realizations = getRealizations().ToArray();
        }

        protected abstract void Undo();

        PossibilitySource[] realizations;
        protected abstract PossibilitySource[] getRealizations();

        public void GetPossibilities(List<PossibilityResult> outlist)
        {
            foreach(var real in realizations)
            {
                if (real.Real(out var job, out var uncertain))
                {
                    var stream = new U.DoubleStream(U.InputSize);

                    job();

                    Parent.Introspect(stream);
                    Parent.SerializeSurrounding(stream);

                    Undo();

                    Action unc_real;
                    if (uncertain != null)
                    {
                        unc_real = uncertain.Realization;
                        stream.WriteOneHot(UncertainEvent.IDSpaceLenght, uncertain.GetID);
                    }
                    else
                    {
                        unc_real = PossibilityResult.Empty;
                        stream.WriteEmpty(UncertainEvent.IDSpaceLenght);
                    }

                    outlist.Add(new PossibilityResult(job, unc_real, stream.ToArray()));
                }
            }
        }

        public delegate bool Realization(out Action job, out UncertainEvent uncertain);

        public sealed class PossibilityResult
        {
            public readonly Action Job;
            public readonly Action Uncertain;
            public readonly double[] Position;
            public double Score = double.MinValue;

            public PossibilityResult(Action _job, Action _uncertain, double[] _position, double _score = double.MinValue)
            {
                Job = _job;
                Position = _position;
                Uncertain = _uncertain;
                Score = _score;
            }

            public static void Empty() { }

            private PossibilityResult() { }

            public static PossibilityResult Default(Agent a) => new PossibilityResult();
            //public static PossibilityResult Default(AgentBase a) => HoldPossibility(a);
            public static PossibilityResult HoldPossibility(Agent a)
            {
                var stream = new U.DoubleStream(U.InputSize);

                a.Introspect(stream);
                a.SerializeSurrounding(stream);
                stream.WriteEmpty(UncertainEvent.IDSpaceLenght);

                return new PossibilityResult(() => { }, () => { }, stream.ToArray(), 0.5);
            }
        }
        public sealed class PossibilitySource
        {
            public readonly Realization Real;

            public PossibilitySource(Realization _realization)
            {
                Real = _realization;
            }
        }
        public abstract class UncertainEvent
        {
            protected UncertainEvent(Action _realization) { Realization = _realization; }
            public static int IDSpaceLenght = 1;

            public abstract int GetID { get; }
            public readonly Action Realization;
        }
    }
}
