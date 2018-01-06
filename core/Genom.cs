namespace core
{
    public abstract class Genome
    {
        protected Genome(int index)
        {
            Index = index;
        }
        protected Genome() : this(++counter) { }


        public abstract double Evaluate(double[] inputs);

        static int counter = 0;
        public readonly int Index = -1;
        public override string ToString()
        {
            return $".{Index}";
        }

        public abstract void OnDies(LearnManager lm, Agent baseOfMutation);
        public abstract Genome OnBirth(LearnManager lm, Agent parent);
    }
}
