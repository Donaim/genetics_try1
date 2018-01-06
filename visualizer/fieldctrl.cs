using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using core;
using genetics_try1;
using stdSimpleNeural;

namespace visualizer
{
    public class FieldControl : Control
    {
        public readonly IFieldWithCells F;
        public const int CELLSIZE = 28;
        public FieldControl(IFieldWithCells f)
        {
            F = f;

            Size = new Size(f.XS * CELLSIZE, f.YS * CELLSIZE);
            gfx = BufferedGraphicsManager.Current.Allocate(CreateGraphics(), ClientRectangle);
        }
        BufferedGraphics gfx;
        static readonly Font hungerFont = new Font("Consolas", 8);
        static readonly Brush hungerBrush = Brushes.Black;
        static readonly StringFormat hungerFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        protected override void OnPaint(PaintEventArgs e)
        {
            var map = F.GetMap;
            for (int x = 0; x < F.XS; x++)
            {
                for (int y = 0; y < F.YS; y++)
                {
                    var obj = map[x, y];
                    var rec = new Rectangle(x * CELLSIZE, y * CELLSIZE, CELLSIZE, CELLSIZE);
                    
                    gfx.Graphics.FillRectangle(getBrush(obj), rec);

                    try
                    {
                        if (obj is MyAgent a)
                        {
                                gfx.Graphics.FillRectangle(Brushes.AliceBlue, rec.X, rec.Y, rec.Width, rec.Height * (a.Hunger / (float)MyAgent.MAXHUNGER));
                                gfx.Graphics.DrawString(a.G.ToString(), hungerFont, hungerBrush, rec, hungerFormat);
                        }
                    }
                    catch { }
                }
            }

            gfx.Render();
        }
        static Brush getBrush(NatureObject obj)
        {
            if(obj == null) { return Brushes.Black; }
            switch (obj)
            {
                case MyAgent a: return Brushes.Blue; //agent
                case SimpleFood food: return Brushes.Green; //food

                default: return Brushes.Red;
            }
        }
        protected override void OnPaintBackground(PaintEventArgs pevent) { }
    }
}
