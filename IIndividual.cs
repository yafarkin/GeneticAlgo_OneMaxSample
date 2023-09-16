namespace GeneticAlgoOneMax;

public interface IIndividual
{
    Guid Id { get; }
    double[] Genes { get; }

    IIndividual Cross(IIndividual otherParent);

    void Mutate();

    double Fitness();
}