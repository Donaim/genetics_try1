using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using core;

namespace stdSimpleNeural
{
    public abstract class SquareMovingEatingAgent : Agent
    {
        public readonly new CellFieldSquareMoving Home;
        
        protected override IEnumerable<ActionsBase> getActions()
        {
            yield return new MoveManager(this);
            yield return new EatManager(this);
        }

        public SquareMovingEatingAgent(Field f, Agent parent) : base(f, parent)
        {
            Home = (CellFieldSquareMoving)f;

            moveManager = (MoveManager)actions.First(z => z is MoveManager);
        }

        public const int MAXHUNGER = 50;
        public int Hunger { get; protected set; } = MAXHUNGER;

        bool eat(out SimpleFood lastFood, out int lastHunger, out Action job, out ActionsBase.UncertainEvent unc)
        {
            foreach (var s in Home.GetSurrounding(this, 1))
            {
                if (s.GetID == SimpleFood.id)
                {
                    lastFood = (SimpleFood)s;
                    lastHunger = Hunger;

                    job = () =>
                    {
                        Hunger = Math.Min(MAXHUNGER, Hunger + MAXHUNGER / 2);
                        s.Dispose();

                        //Console.WriteLine($"I ({px},{py}) can eat ({s.PX},{s.PY})");
                    };
                    unc = new EatManager.SpawnFoodUncertain(
                    () =>
                    {
                        SimpleFood.SpawnRandomNormal(Home);
                        //Console.WriteLine("Ate something..");
                    });
                    //unc = new EatManager.SpawnFoodUncertain(() => My_FoodObj.SpawnRandom(Home));

                    return true;
                }
            }

            job = null; unc = null;
            lastHunger = -1;
            lastFood = null;
            return false;
        }
        
        MoveManager moveManager;
        public class MoveManager : ActionsBase
        {
            public readonly new SquareMovingEatingAgent Parent;
            public MoveManager(SquareMovingEatingAgent b) : base(b) { Parent = b; }

            public CellFieldSquareMoving.Direction LastDir { get; private set; } = CellFieldSquareMoving.Direction.Undefined;
            int lastX, lastY;
            public bool MoveLeft(out Action job, out UncertainEvent unc)
            {
                LastDir = CellFieldSquareMoving.Direction.Left;
                lastX = Parent.PX; lastY = Parent.PY;

                unc = null;
                job = () => Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Left);

                var re = Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Left);
                if (re) { Undo(); }

                return re;
            }
            private bool MoveUp(out Action job, out UncertainEvent unc)
            {
                LastDir = CellFieldSquareMoving.Direction.Up;
                lastX = Parent.PX; lastY = Parent.PY;

                unc = null;
                job = () => Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Up);

                var re = Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Up);
                if (re) { Undo(); }

                return re;
            }
            private bool MoveDown(out Action job, out UncertainEvent unc)
            {
                LastDir = CellFieldSquareMoving.Direction.Down;
                lastX = Parent.PX; lastY = Parent.PY;

                unc = null;
                job = () => Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Down);

                var re = Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Down);
                if (re) { Undo(); }

                return re;
            }
            private bool MoveRight(out Action job, out UncertainEvent unc)
            {
                LastDir = CellFieldSquareMoving.Direction.Right;
                lastX = Parent.PX; lastY = Parent.PY;

                unc = null;
                job = () => Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Right);

                var re = Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Right);
                if (re) { Undo(); }

                return re;
            }

            protected override PossibilitySource[] getRealizations()
            {
                return new PossibilitySource[]
                {
                    new PossibilitySource(MoveRight),
                    new PossibilitySource(MoveLeft),
                    new PossibilitySource(MoveDown),
                    new PossibilitySource(MoveUp),
                };
            }

            protected override void Undo()
            {
                //Parent.moveInternal(lastX, lastY);
                switch (LastDir)
                {
                    case CellFieldSquareMoving.Direction.Left:
                        if (!Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Right)) { throw new Exception("CANNOT UNDO MOVE!"); }
                        break;
                    case CellFieldSquareMoving.Direction.Right:
                        if (!Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Left)) { throw new Exception("CANNOT UNDO MOVE!"); }
                        break;
                    case CellFieldSquareMoving.Direction.Up:
                        if (!Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Down)) { throw new Exception("CANNOT UNDO MOVE!"); }
                        break;
                    case CellFieldSquareMoving.Direction.Down:
                        if (!Parent.Home.MoveObject(Parent, CellFieldSquareMoving.Direction.Up)) { throw new Exception("CANNOT UNDO MOVE!"); }
                        break;
                    case CellFieldSquareMoving.Direction.Undefined: throw new Exception("Tried to undo undefined! Problem must be that Undo() was called too many times");
                    default: throw new NotImplementedException();
                }
                //LastDir = MyField.Direction.Undefined;
            }
        }
        public class EatManager : ActionsBase
        {
            public new SquareMovingEatingAgent Parent;
            public EatManager(SquareMovingEatingAgent parent) : base(parent)
            {
                Parent = parent;
            }

            protected override PossibilitySource[] getRealizations()
            {
                return new PossibilitySource[]
                {
                    new PossibilitySource(eat),
                };
            }

            int lastHunger;
            SimpleFood lastFood;
            bool eat(out Action job, out UncertainEvent unc)
            {
                return Parent.eat(out lastFood, out lastHunger, out job, out unc);
            }

            protected override void Undo()
            {
                Parent.Hunger = lastHunger;
                if (!lastFood.Reincarnate()) { throw new Exception("Cannot reincarnate last eaten food!!"); }
            }

            public class SpawnFoodUncertain : UncertainEvent
            {
                public static int uid = 0;
                public override int GetID => uid;

                public SpawnFoodUncertain(Action _realization) : base(_realization) { }
            }
        }
    }
}
