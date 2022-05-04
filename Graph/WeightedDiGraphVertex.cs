namespace PlatformTest.Graph;

public class WeightedDiGraphVertex
{
    public int Key { get; set; }

    public Dictionary<WeightedDiGraphVertex, DiEdge> OutEdges { get; }
    public Dictionary<WeightedDiGraphVertex, DiEdge> InEdges { get; }

    public WeightedDiGraphVertex(int value)
    {
        Key = value;

        OutEdges = new Dictionary<WeightedDiGraphVertex, DiEdge>();
        InEdges = new Dictionary<WeightedDiGraphVertex, DiEdge>();
    }
    
    public DiEdge GetEdge(WeightedDiGraphVertex targetVertex)
    {
        var key = targetVertex;
        return OutEdges[key];
    }
}