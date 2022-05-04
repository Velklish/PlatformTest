using PlatformTest.Graph;

namespace PlatformTest;

public class DiEdge
{
    public int Key { get; }
    
    internal DiEdge(WeightedDiGraphVertex source, WeightedDiGraphVertex target, int key)
    {
        TargetVertex = target;
        SourceVertex = source;
        Key = key;
    }

    public int TargetVertexKey => TargetVertex.Key;

    public WeightedDiGraphVertex TargetVertex { get; }
    
    public WeightedDiGraphVertex SourceVertex { get; }
}