using System.Collections.ObjectModel;
using System.Drawing;
using ScottPlot;

namespace GeneticAlgoOneMax;

public class GeneticAlgoEngine
{
    protected Random _random = new();
    protected const int _tournamentCount = 3;

    public List<IIndividual> Population { get; protected set; }

    public GeneticAlgoEngine(List<IIndividual> population)
    {
        Population = population;
    }

    public IIndividual Evolute(int generations, double expectedMaxFitness, double crossOverProbability, double mutationProbability)
    {
        if (0 == Population.Count)
        {
            throw new InvalidOperationException("Nothing to evaluate");
        }

        var maxValues = new List<double>();
        var avgValues = new List<double>();

        var i = 0;
        while (true)
        {
            if (i++ >= generations)
            {
                break;
            }

            var best = FindBest(Population);
            var bestFitness = best.Fitness();
            if (bestFitness >= expectedMaxFitness)
            {
                return best;
            }

            var avgFitness = Population.Average(_ => _.Fitness());
            Console.WriteLine($"{i}. best = {bestFitness}. avg = {avgFitness}");

            maxValues.Add(bestFitness);
            avgValues.Add(avgFitness);

            var newPopulation = new List<IIndividual>(Population.Count);
            for (var y = 0; y < Population.Count; y++)
            {
                newPopulation.Add(Selection(Population));
            }

            for (var y = 0; y < newPopulation.Count; y += 2)
            {
                if (_random.NextDouble() < crossOverProbability)
                {
                    var parent1 = newPopulation[y];
                    var parent2 = newPopulation[y + 1];

                    var child1 = parent1.Cross(parent2);
                    var child2 = parent2.Cross(parent1);

                    newPopulation[y] = child1;
                    newPopulation[y + 1] = child2;
                }
            }

            foreach (var individual in newPopulation)
            {
                if (_random.NextDouble() < mutationProbability)
                {
                    individual.Mutate();
                }
            }

            Population = newPopulation;
        }

        var plot = new Plot(1600, 900);
        plot.XLabel("Поколение");
        plot.XAxis.Color(Color.Red);
        plot.YLabel("Макс/срд. приспособленность");
        plot.YAxis.Color(Color.Green);
        plot.Title("Поиск решения по поколениям");
        plot.AddSignal(maxValues.ToArray());
        plot.AddSignal(avgValues.ToArray());
        plot.SaveFig("evolute.png");

        var result = FindBest(Population);
        return result;
    }

    protected IIndividual Selection(IList<IIndividual> population)
    {
        return TournamentSelection(population);
    }

    protected IIndividual TournamentSelection(IList<IIndividual> population)
    {
        IList<IIndividual> candidates = new List<IIndividual>();
        if (_tournamentCount >= population.Count)
        {
            candidates = population;
        }
        else
        {
            while (candidates.Count < _tournamentCount)
            {
                var i = _random.Next(population.Count);
                var candidate = population[i];
                if (candidates.Any(_ => _.Id == candidate.Id))
                {
                    continue;
                }

                candidates.Add(candidate);
            }
        }

        var best = FindBest(candidates);
        return best;
    }

    protected IIndividual FindBest(IEnumerable<IIndividual> population)
    {
        IIndividual? result = null;
        var fitness = double.MinValue;
        foreach (var individual in population)
        {
            if (individual.Fitness() > fitness)
            {
                result = individual;
                fitness = individual.Fitness();
            }
        }

        if (null == result)
        {
            throw new InvalidOperationException("Can't find the best individual, may be sequence is empty?");
        }

        return result;
    }
}