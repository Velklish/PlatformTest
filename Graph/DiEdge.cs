namespace PlatformTest;

public class DiEdge
{
    public Bus Performer;
    private IStrategy _weightCalculationStrategy;

    internal DiEdge(WeightedDiGraphVertex source, WeightedDiGraphVertex target, Bus performer, IStrategy strategy)
    {
        TargetVertex = target;
        SourceVertex = source;
        Performer = performer;
        _weightCalculationStrategy = strategy;
    }

    public int TargetVertexKey => TargetVertex.Key;

    public WeightedDiGraphVertex TargetVertex { get; }
    
    public WeightedDiGraphVertex SourceVertex { get; }

    public int Weight(CalculationContext context)
    {
        return _weightCalculationStrategy.CalculateParameter(context, this);
    }
}