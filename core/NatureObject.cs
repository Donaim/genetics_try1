using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public abstract class NatureObject : IDisposable
    {
        public static int IDSpaceCount = 3;

        public readonly Field Home;
        protected NatureObject(Field enviroment, Func<double, double> coorsFunc, bool exists = true)
        {
            Home = enviroment;

            Exists = exists;
            if (Exists)
            {
                do { chooseCoors(coorsFunc); }
                while (!Home.AddObject(this)) ;
            }
        }
        protected NatureObject()
        {
            Exists = false;
            px = -1; py = -1;
        }
        protected NatureObject(Field enviroment, bool exists = true) : this(enviroment, stdCoorsFunc, exists) { }

        public static double stdCoorsFunc(double x) => x;
        void chooseCoors(Func<double, double> f)
        {
            for(int i = 0; i < 100; i++)
            {
                px = (int)(f(U.Rand()) * Home.XS);
                py = (int)(f(U.Rand()) * Home.YS);
                if((px >= 0 && py >= 0 && px < Home.XS && py < Home.YS)) { return; }
            }

            throw new Exception("Very bad function for initializing coors... Or there is just no more place for another object on the map");
        }

        public virtual void Serialize(U.DoubleStream stream) => stream.WriteOneHot(IDSpaceCount, GetID);
        public abstract int GetID { get; }

        internal int px, py;
        public int PX => px;
        public int PY => py;

        public bool Exists { get; private set; } = false;
        public void Dispose()
        {
            if (Exists)
            {
                Home.RemoveObject(this);
                Exists = false;
            }
        }
        public bool Reincarnate()
        {
            if (Home.AddObject(this))
            {
                Exists = true;
                return true;
            }
            else { return false; }
        }
    }
    public class MapBoarder : NatureObject
    {
        public MapBoarder(Field env) : base(env, false) { }

        static readonly int id = 0;
        public override int GetID => id;

        public override void Serialize(U.DoubleStream stream) => stream.WriteEmpty(IDSpaceCount);
    }
    public class ZeroSpace : NatureObject
    {
        public ZeroSpace(Field env) : base(env, false) { }

        static readonly int id = -1;
        public override int GetID => id;

        //public override double[] Serialize() => new double[IDSpaceCount];
        public override void Serialize(U.DoubleStream stream) => stream.WriteEmpty(IDSpaceCount);
    }

}
