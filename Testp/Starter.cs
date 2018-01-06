using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

using core;
using visualizer;
using genetics_try1;
using stdSimpleNeural;

using System.Windows.Forms;


namespace Testp
{
    public static class Starter
    {
        public static  void Go(Field field, LearnManager lm, List<Agent> agents)
        {
            Console.WriteLine("START");

            var ctrl = new FieldControl((IFieldWithCells)field);

            var form = new Form();
            form.Controls.Add(ctrl);
            ctrl.Location = new System.Drawing.Point(10, 10);
            form.Size = new System.Drawing.Size(ctrl.Width + 40, ctrl.Height + 60);
            form.KeyPreview = true;
            form.KeyUp += (s, e) =>
            {
                if (e.KeyCode == Keys.Right) { compute_fps = Math.Max(0, compute_fps - 10); }
                else if (e.KeyCode == Keys.Left) { compute_fps = Math.Min(300, compute_fps + 10); }
                else if (e.KeyCode == Keys.Space) { computePause = !computePause; }
                form.Text = $"{nameof(compute_fps)} = {1000 / Math.Max(double.Epsilon, compute_fps)}";
            };
            form.Text = $"{nameof(compute_fps)} = {1000 / Math.Max(double.Epsilon, compute_fps)}";

            engine(agents, form, lm);

            Application.Run(form);
        }

        static void engine(List<Agent> agents, Form f, LearnManager lm)
        {
            new Thread(computeMy).Start();
            new Thread(drawMy).Start();
            new Thread(reportMy).Start();

            void computeMy() { compute(agents, lm); }
            void drawMy() { draw(f); }
            void reportMy() { report(agents, lm); }
        }

        static bool computePause = false;
        static int compute_fps = 100;
        static void compute(List<Agent> agents, LearnManager lm)
        {
            var home = agents[0].Home;

            for (int tick = 0; true; tick++)
            {
                Thread.Sleep(compute_fps);
                while (computePause) { Thread.Sleep(1); }
                try
                {
                    for (int i = agents.Count - 1; i >= 0; i--)
                    {
                        agents[i].PreDo();
                        if (agents[i].Dead)
                        {
                            agents[i] = ((MyAgent)agents[i]).BornNew(home);
                            //agents.RemoveAt(i);
                        }
                        else { agents[i].Do(); }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }

            }
        }
        static void draw(Form f)
        {
            while (true)
            {
                Thread.Sleep(1000 / 30);

                if (f.Created)
                {
                    try
                    {
                        f.Invoke((Action)f.Refresh);
                    }
                    catch { break; }
                }
            }

            compute_fps = 0;
        }
        static int lastDeath = 0;
        static void report(List<Agent> agents, LearnManager lm)
        {
            string str = "";
            //var toplist = new ModuloList<MyGenom>(10);
            int global_max = int.MinValue;


            while (true)
            {
                Thread.Sleep(300);
                while (computePause) { Thread.Sleep(1); }

                int max_on_cycle = (int)(1.0 / lm.GetMaxRatio(1));
                if (max_on_cycle > global_max)
                {
                    global_max = max_on_cycle;
                    //toplist.SetNext(lm.GetGenoms.Max(z => z.)
                }

                if (lm.DeathCount < lastDeath)
                {

                }

                print($"Death count:          {lm.DeathCount}");
                print($"Average live time:    {lm.GetAverage()}");
                print($"Max live time:        {max_on_cycle}");
                print($"Global max live time: {global_max}");
                print($"");

                Console.WriteLine(str); str = "";
            }

            void print(string s) { str += s + '\n'; }
        }
    }
}
