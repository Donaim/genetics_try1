using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public abstract class Field
    {
        public readonly int XS, YS;
        public Field(int xs, int ys)
        {
            XS = xs; YS = ys;

            boarderObj = new MapBoarder(this);
            zeroObj = new ZeroSpace(this);
        }

        protected void changeObjectCoors(NatureObject obj, int newX, int newY)
        {
            obj.px = newX; obj.py = newY;
        }

        public abstract bool AddObject(NatureObject obj);
        public abstract bool ReplaceObject(NatureObject o0, NatureObject o1);
        public void RemoveObject(NatureObject obj) => ReplaceObject(obj, null);

        protected readonly MapBoarder boarderObj;
        protected readonly ZeroSpace zeroObj;
        public abstract void SerializeSurrounding(Agent a, int rad, U.DoubleStream stream);
        public abstract IEnumerable<NatureObject> GetSurrounding(Agent a, int rad);
    }
}
