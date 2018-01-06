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
    // [TestClass]
    public class First
    {
        // [TestMethod]
        public void test_field_ctrl1()
        {
            U.InputSize = MyAgent.SurroundingLen *  NatureObject.IDSpaceCount + MyAgent.IntrospectLen + ActionsBase.UncertainEvent.IDSpaceLenght;

            var field = new MyCellField(47, 23);
            var ctrl = new FieldControl(field);

            var lm = new MyLearnManager();

            var agents = new List<Agent>();
            for(int i = 0; i < 320; i++) { SimpleFood.SpawnRandomNormal(field); }
            for(int i = 0; i < 10; i++) { agents.Add(lm.CreateNew(field, typeof(MyGenome), typeof(MyAgent))); }

            Starter.Go(field, lm, agents);
        }
    }
}
