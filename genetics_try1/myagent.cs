using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Math;

using stdSimpleNeural;
using core;
using VNNLib;

namespace genetics_try1
{
    public class MyAgent : SquareMovingEatingAgent, ITrackHistory
    {
        public static readonly int id = 1;
        public override int GetID => id;

        public MyAgent(Field f, Agent p) : base(f, p) { }


        public Agent BornNew(Field home) => new MyAgent(home, this);
        
        public int Age { get; private set; } = 1;
        public override double GetFitness() => Age;

        public override void PreDo()
        {
            Age++;
            if (Hunger > 1) { Hunger--; }
            else
            {
                //Console.WriteLine($"MyAgent died on ({px},{py})");
                Die();
            }
        }

        //static readonly int movesCount = Enum.GetNames(typeof(MyField.Direction)).Length;
        static readonly int movesCount = 4;
        public static readonly int IntrospectLen = 2;//+ movesCount;
        public override void Introspect(U.DoubleStream stream)
        {
            stream.Write(1 - 1.0 / (double)Hunger);
            stream.Write(1 - 1.0 / (double)Age);

            //stream.WriteOneHot(movesCount, (int)moveManager.LastDir);
        }

        const int RAD = 1;
        public static readonly int SurroundingLen = (RAD * 2 + 1) * (RAD * 2 + 1);
        public override void SerializeSurrounding(U.DoubleStream stream)
        {
            Home.SerializeSurrounding(this, RAD, stream);
        }

        public U.ModuloList<EvaluateUnit> History { get; set; } = new U.ModuloList<EvaluateUnit>(10000);
        public override double Evaluate(double[] inputs)
        {
            var re = base.Evaluate(inputs);
            History.SetNext(new EvaluateUnit(inputs, re));
            return re;
        }
        public void CleanHistory()
        {
            History = null;
        }
    }
}
