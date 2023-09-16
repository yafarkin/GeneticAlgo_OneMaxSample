namespace GeneticAlgoOneMax;

public abstract class BaseIndividual : IIndividual
{
    protected Random _random = new();

    protected virtual double _mutateChallenge => 0.99;
    protected abstract int _genesLength { get; }

    protected Guid _id => Guid.NewGuid();

    public Guid Id => _id;

    public double[] Genes { get; protected set; }

    protected BaseIndividual()
    {
        Genes = new double[_genesLength];
        for (var i = 0; i < Genes.Length; i++)
        {
            Genes[i] = _random.NextDouble();
        }
    }

    protected BaseIndividual(double[] genes)
    {
        Genes = genes;
    }

    public virtual IIndividual Cross(IIndividual otherParent)
    {
        if (otherParent.Genes.Length != Genes.Length)
        {
            throw new InvalidOperationException("Genes count different between parents");
        }

        var thisCrossPoint = _random.Next(1, Genes.Length - 2);

        var thisLength = thisCrossPoint;
        var otherLength = otherParent.Genes.Length - thisCrossPoint;

        var newGenes = new double[thisLength + otherLength];

        Array.Copy(Genes, newGenes, thisLength);
        Array.Copy(otherParent.Genes, thisLength, newGenes, thisLength, otherLength);

        var newIndividual = CreateNewIndividual(newGenes);
        return newIndividual;
    }

    public virtual void Mutate()
    {
        for (var i = 0; i < Genes.Length; i++)
        {
            if (_random.NextDouble() < _mutateChallenge)
            {
                continue;
            }

            Genes[i] = MutateGene(i, Genes[i]);
        }
    }

    protected abstract IIndividual CreateNewIndividual(double[] genes);

    protected abstract double MutateGene(int i, double gene);

    public abstract double Fitness();
}