using static ProblemPlecakowy.KnapsackDK;

class Program
{
    static void Main(string[] args)
    {
        Knapsack knapsack = InitializeKnapsack();
        GeneticAlgorithm ga = InitializeGeneticAlgorithm(knapsack);

        Chromosome bestSolution = ga.Run();

        DisplaySolution(bestSolution, knapsack);
    }

    static Knapsack InitializeKnapsack()
    {
        return new Knapsack
        {
            Capacity = 100,
            Items = new List<Item>
            {
                new Item { Weight = 20, Value = 120 },
                new Item { Weight = 40, Value = 200 },
                new Item { Weight = 60, Value = 240 }
            }
        };
    }

    static GeneticAlgorithm InitializeGeneticAlgorithm(Knapsack knapsack)
    {
        int populationSize = 10;
        double crossoverRate = 0.8;
        double mutationRate = 0.05;
        int generations = 100;

        return new GeneticAlgorithm(populationSize, crossoverRate, mutationRate, generations, knapsack);
    }

    static void DisplaySolution(Chromosome bestSolution, Knapsack knapsack)
    {
        Console.WriteLine("Najlepsze rozwiązanie:");
        for (int i = 0; i < bestSolution.Genes.Length; i++)
        {
            if (bestSolution.Genes[i])
            {
                Console.WriteLine($"Przedmiot {i + 1}: Waga = {knapsack.Items[i].Weight}, Wartość = {knapsack.Items[i].Value}");
            }
        }
        Console.WriteLine($"Całkowita wartość: {bestSolution.Fitness}");
    }
}