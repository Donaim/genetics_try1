using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace core
{
    public abstract class LearnManager
    {
        double[] fitness_list;
        Genome[] genom_list;
        int index = 0;
        bool initialized = false;

        public LearnManager(int list_size)
        {
            fitness_list = new double[list_size];
            genom_list = new Genome[list_size];
        }

        Genome onBornPrivate(Agent child)
        {
            NewMember(child);
            return onBorn(child);
        }
        protected abstract Genome onBorn(Agent child);
        public Agent CreateNew(Field enviroment, Type genotype, Type prototype)
        {
            var c = createNew(enviroment, genotype, prototype);
            NewMember(c);
            return c;
        }
        protected abstract Agent createNew(Field enviroment, Type genotype, Type prototype);
        void NewMember(Agent a)
        {
            a.OnDie += () => Dies(a);
            a.OnBorn += (child) => onBornPrivate(child);

            //if (!genoms.ContainsKey(a.G.Index))
            //{
            //    genoms.Add(a.G.Index, a.G);
            //    //Console.WriteLine($"New genom {g} added to population");
            //}
            //else
            //{
            //    Console.WriteLine($"Existing genom {g} population is {g.Population} now");
            //}
        }

        public int DeathCount { get; private set; } = 0;
        public void Dies(Agent a)
        {
            DeathCount++;

            fitness_list[index] = a.GetFitness();
            genom_list[index] = a.G;

            index = (index + 1) % fitness_list.Length;
            if (index == 0) { initialized = true; }

            onDies(a);

            //if(genoms[a.G.Index].Population <= 0)
            //{
            //    genoms.Remove(a.G.Index);
            //    //Console.WriteLine($"Deleted {a.G} genom");
            //}
        }
        protected abstract void onDies(Agent a);

        public Genome GetWagedRandomGenome(int factor = 2)
        {
            int n = (initialized ? fitness_list.Length : index);
            ulong sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += (ulong)Math.Pow(fitness_list[i], factor);
            }
            ulong random_sum = (ulong)(U.Rand() * sum);
            sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += (ulong)Math.Pow(fitness_list[i], factor);
                if (sum >= random_sum) { return genom_list[i]; }
            }

            throw new Exception("WTF?");
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public double GetAverage()
        {
            double sum = 0.0;
            int n = (initialized ? fitness_list.Length : index);
            for (int i = 0; i < n; i++)
            {
                sum += fitness_list[i];
            }
            return sum / n;
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public double GetMaxRatio(double fit)
        {
            double max = fitness_list[0];
            for (int i = 1; i < fitness_list.Length; i++) { if (fitness_list[i] > max) { max = fitness_list[i]; } }
            return fit / max;
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public double GetMeanRatio(double fit) => fit / GetAverage();
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public double GetMeanRatioNormalized(double fit) => my_sigmoid(fit / GetAverage());

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //protected static double my_sigmoid(double x) => x > 0.5 ? 1 : 0;
        protected static double my_sigmoid(double x) => 1 / (1 + Math.Exp(-16 * x + 8));
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static double GetCorrect(double pred, double good)
        {
            var g = my_sigmoid(good);
            return ((1 - g) + pred * (g * 2 - 1));
        }
    }
}
