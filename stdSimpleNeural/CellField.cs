using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using core;

namespace stdSimpleNeural
{
    public interface IFieldWithCells
    {
        NatureObject[,] GetMap { get; }
        int XS { get; }
        int YS { get; }
    }
    public abstract class CellField : Field, IFieldWithCells
    {
        public CellField(int xs, int ys) : base(xs, ys)
        {
            Map = new NatureObject[xs, ys];
        }

        public readonly NatureObject[,] Map;
        public NatureObject[,] GetMap => Map;
        public int XS => base.XS;
        public int YS => base.YS;

        public override sealed bool AddObject(NatureObject obj)
        {
            if (Map[obj.PX, obj.PY] != null) { return false; }
            else
            {
                Map[obj.PX, obj.PY] = obj;
                return true;
            }
        }
        public override sealed bool ReplaceObject(NatureObject o0, NatureObject o1)
        {
            Map[o0.PX, o0.PY] = o1;
            return true;
        }
    }
    public abstract class CellFieldSquareMoving : CellField
    {
        public CellFieldSquareMoving(int xs, int ys) : base(xs, ys)
        {

        }

        public enum Direction { Left, Right, Up, Down, LeftUp, RightUp, LeftDown, RightDown, Undefined }
        public bool MoveObject(NatureObject obj, Direction dir)
        {
            int newX = obj.PX, oldX = newX, newY = obj.PY, oldY = newY;

            switch (dir)
            {
                case Direction.Left: newX--; break;
                case Direction.Right: newX++; break;
                case Direction.Up: newY--; break;
                case Direction.Down: newY++; break;
                default: throw new NotImplementedException();
            }

            if (newX < 0) { newX = XS + newX; }
            else { newX = newX % XS; }
            if (newY < 0) { newY = YS + newY; }
            else { newY = newY % YS; }

            if (Map[newX, newY] != null) { return false; }
            else
            {
                Map[oldX, oldY] = null;
                changeObjectCoors(obj, newX, newY);
                Map[newX, newY] = obj;

                return true;
            }
        }
    }
}
