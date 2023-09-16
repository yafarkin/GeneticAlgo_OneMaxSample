namespace GeneticAlgoOneMax;

public class OneMaxIndividual : BaseIndividual
{
    protected override int _genesLength => 100;

    public OneMaxIndividual()
    {
        for (var i = 0; i < Genes.Length; i++)
        {
            Genes[i] = _random.NextDouble() >= 0.5 ? 1 : 0;
        }
    }

    public OneMaxIndividual(double[] genes)
        : base(genes)
    {
    }

    protected override IIndividual CreateNewIndividual(double[] genes)
    {
        return new OneMaxIndividual(genes);
    }

    protected override double MutateGene(int i, double gene)
    {
        return gene > 0 ? 0 : 1;
    }

    public override double Fitness()
    {
        return Genes.Sum(_ => _);
    }
}