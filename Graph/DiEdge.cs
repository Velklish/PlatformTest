namespace PlatformTest.Graph;

/// <summary> Ребро графа. </summary>
public class DiEdge
{
    public int Key { get; }
    
    internal DiEdge(WeightedDiGraphVertex source, WeightedDiGraphVertex target, int key)
    {
        TargetVertex = target;
        SourceVertex = source;
        Key = key;
    }

    public WeightedDiGraphVertex TargetVertex { get; }
    
    public WeightedDiGraphVertex SourceVertex { get; }
}