using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using core;
using stdSimpleNeural;

namespace genetics_try1
{
    public class MyCellField : CellFieldSquareMoving
    {
        public MyCellField(int xs, int ys) : base(xs, ys)
        {
        }

        public override void SerializeSurrounding(Agent a, int rad, U.DoubleStream stream)
        {
            for (int x = 0, xstart = a.PX - rad, xend = rad * 2; x <= xend; x++)
            {
                for (int y = 0, ystart = a.PY - rad, yend = rad * 2; y <= yend; y++)
                {
                    int xi = xstart + x, yi = ystart + x;

                    if (xi < 0) { xi = XS + xi; }
                    else { xi = xi % XS; }
                    if (yi < 0) { yi = YS + yi; }
                    else { yi = yi % YS; }

                    if (Map[xi, yi] == null) { stream.WriteEmpty(NatureObject.IDSpaceCount); }
                    else { Map[xi, yi].Serialize(stream); }
                }
            }
        }
        public override IEnumerable<NatureObject> GetSurrounding(Agent a, int rad)
        {
            for (int x = 0, xstart = a.PX - rad, xend = rad * 2; x <= xend; x++)
            {
                for (int y = 0, ystart = a.PY - rad, yend = rad * 2; y <= yend; y++)
                {
                    int xi = xstart + x, yi = ystart + x;

                    if (xi < 0) { xi = XS + xi; }
                    else { xi = xi % XS; }
                    if (yi < 0) { yi = YS + yi; }
                    else { yi = yi % YS; }

                    //if (x >= XS || x < 0 || y >= YS || y < 0) { }
                    if (Map[xi, yi] != null) { yield return Map[xi, yi]; }
                }
            }
        }
    }
}
