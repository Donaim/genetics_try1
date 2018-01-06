using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using core;

namespace stdSimpleNeural
{
    public class SimpleFood : NatureObject
    {
        public static readonly int id = 2;
        public override int GetID => id;

        public SimpleFood(Agent a) : this(a.Home) { }
        public SimpleFood(Field f) : this(f, stdCoorsFunc) { }
        public SimpleFood(Field f, Func<double, double> coorsFunc) : base(f, coorsFunc)
        {
            //Console.WriteLine($"Food created on ({px},{py})");
            Count++;
        }

        public static int Count { get; set; } = 0;

        public static SimpleFood SpawnRandom(Field f)
        {
            return new SimpleFood(f);
            //return me;
        }

        public static SimpleFood SpawnRandomNormal(Field f)
        {
            //return null;
            const ushort pow = 1;
            //double normal(double x) => Math.Pow(x * 2 - 1, 2 * pow + 1) / 2.0 + 0.5;
            double normal(double x) => Math.Sqrt(Math.Log(1 / x));

            var me = new SimpleFood(f, normal);
            return me;
        }
    }
}
