using GeneticAlgoOneMax;

var population = new List<IIndividual>();
for (var i = 0; i < 100; i++)
{
    population.Add(new OneMaxIndividual());
}

var engine = new GeneticAlgoEngine(population);
var best = engine.Evolute(50, 100, 0.9, 0.1);
Console.WriteLine(best.Fitness());