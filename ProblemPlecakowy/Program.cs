namespace ProblemPlecakowy
{
    public class KnapsackDK
    {
        private static Random random = new Random();

        public class Item
        {
            public int Weight { get; set; }
            public int Value { get; set; }
        }

        public class Chromosome
        {
            public bool[] Genes { get; set; }
            public int Fitness { get; set; }
        }

        public class Knapsack
        {
            public int Capacity { get; set; }
            public List<Item> Items { get; set; }
        }

        public class GeneticAlgorithm
        {
            private int populationSize;
            private double crossoverRate;
            private double mutationRate;
            private int generations;
            private Knapsack knapsack;

            public GeneticAlgorithm(int populationSize, double crossoverRate, double mutationRate, int generations, Knapsack knapsack)
            {
                this.populationSize = populationSize;
                this.crossoverRate = crossoverRate;
                this.mutationRate = mutationRate;
                this.generations = generations;
                this.knapsack = knapsack;
            }

            public Chromosome Run()
            {
                var population = InitializePopulation();
                EvaluatePopulation(population);

                for (int generation = 0; generation < generations; generation++)
                {
                    population = EvolvePopulation(population);
                }

                return population.OrderByDescending(c => c.Fitness).First();
            }

            private List<Chromosome> InitializePopulation()
            {
                return Enumerable.Range(0, populationSize).Select(_ => CreateRandomChromosome()).ToList();
            }

            private Chromosome CreateRandomChromosome()
            {
                var chromosome = new Chromosome
                {
                    Genes = new bool[knapsack.Items.Count]
                };

                for (int i = 0; i < knapsack.Items.Count; i++)
                {
                    chromosome.Genes[i] = random.NextDouble() < 0.5;
                }

                return chromosome;
            }

            private void EvaluatePopulation(List<Chromosome> population)
            {
                foreach (var chromosome in population)
                {
                    EvaluateChromosome(chromosome);
                }
            }

            private void EvaluateChromosome(Chromosome chromosome)
            {
                var (totalWeight, totalValue) = CalculateWeightAndValue(chromosome);

                chromosome.Fitness = totalWeight <= knapsack.Capacity ? totalValue : 0;
            }

            private (int totalWeight, int totalValue) CalculateWeightAndValue(Chromosome chromosome)
            {
                int totalWeight = 0;
                int totalValue = 0;

                for (int i = 0; i < chromosome.Genes.Length; i++)
                {
                    if (chromosome.Genes[i])
                    {
                        totalWeight += knapsack.Items[i].Weight;
                        totalValue += knapsack.Items[i].Value;
                    }
                }

                return (totalWeight, totalValue);
            }

            private List<Chromosome> EvolvePopulation(List<Chromosome> population)
            {
                var newPopulation = new List<Chromosome>();

                while (newPopulation.Count < populationSize)
                {
                    var (offspring1, offspring2) = CreateOffspring(SelectParent(population), SelectParent(population));

                    newPopulation.Add(offspring1);
                    newPopulation.Add(offspring2);
                }

                EvaluatePopulation(newPopulation);

                return newPopulation.OrderByDescending(c => c.Fitness).Take(populationSize).ToList();
            }

            private Chromosome SelectParent(List<Chromosome> population)
            {
                int tournamentSize = 3;
                return Enumerable.Range(0, tournamentSize)
                                 .Select(_ => population[random.Next(population.Count)])
                                 .OrderByDescending(c => c.Fitness)
                                 .First();
            }

            private (Chromosome, Chromosome) CreateOffspring(Chromosome parent1, Chromosome parent2)
            {
                var (offspring1, offspring2) = Crossover(parent1, parent2);

                Mutate(offspring1);
                Mutate(offspring2);

                return (offspring1, offspring2);
            }

            private (Chromosome offspring1, Chromosome offspring2) Crossover(Chromosome parent1, Chromosome parent2)
            {
                var offspring1 = new Chromosome { Genes = new bool[parent1.Genes.Length] };
                var offspring2 = new Chromosome { Genes = new bool[parent2.Genes.Length] };

                if (random.NextDouble() < crossoverRate)
                {
                    int crossoverPoint = random.Next(parent1.Genes.Length);

                    for (int i = 0; i < crossoverPoint; i++)
                    {
                        offspring1.Genes[i] = parent1.Genes[i];
                        offspring2.Genes[i] = parent2.Genes[i];
                    }

                    for (int i = crossoverPoint; i < parent1.Genes.Length; i++)
                    {
                        offspring1.Genes[i] = parent2.Genes[i];
                        offspring2.Genes[i] = parent1.Genes[i];
                    }
                }
                else
                {
                    Array.Copy(parent1.Genes, offspring1.Genes, parent1.Genes.Length);
                    Array.Copy(parent2.Genes, offspring2.Genes, parent2.Genes.Length);
                }

                return (offspring1, offspring2);
            }

            private void Mutate(Chromosome chromosome)
            {
                for (int i = 0; i < chromosome.Genes.Length; i++)
                {
                    if (random.NextDouble() < mutationRate)
                    {
                        chromosome.Genes[i] = !chromosome.Genes[i];
                    }
                }
            }
        }
    }
}