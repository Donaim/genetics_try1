using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using core;
using genetics_try1;
using stdSimpleNeural;

namespace family_genetics
{
    class FamilyAgent : SquareMovingEatingAgent 
    {
        public FamilyAgent(Field f, MyAgent parent) : base(f, parent)
        {
        }

        public override int GetID => throw new NotImplementedException();

        public override double GetFitness()
        {
            throw new NotImplementedException();
        }

        public override void Introspect(U.DoubleStream stream)
        {
            throw new NotImplementedException();
        }

        public override void SerializeSurrounding(U.DoubleStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
