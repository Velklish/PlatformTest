namespace PlatformTest.Graph;

/// <summary>
/// A weighted graph implementation.
/// IEnumerable enumerates all vertices.
/// </summary>
public class WeightedDiGraph
{
    public IStrategy CalculateStrategy { get; set; }
    internal Dictionary<int, WeightedDiGraphVertex> Vertices { get; set; }

    public int EdgeWeight(Dictionary<int, int> parentMap, int source, int current, int target)
    {
        var parts = this.GetPath(parentMap, source, current);
        
        parts.Add(new IStrategy.PathPart()
        {
            EdgeId = Vertices[target].InEdges[Vertices[current]].Key,
            SourceVertexId = current,
            TargetVerexId = target
        });

        return CalculateStrategy.CalculateParameter(parts);
    }

    public List<IStrategy.PathPart> GetPath(Dictionary<int, int> parentMap, int source, int current)
    { 
        var parts = new List<IStrategy.PathPart>();
        
        while (current != source)
        {
            var prev = parentMap[current];
            var edge = Vertices[current].InEdges[Vertices[prev]];
            parts.Add(new IStrategy.PathPart
            {
                EdgeId = edge.Key,
                SourceVertexId = prev,
                TargetVerexId = current
            });

            current = prev;
        }

        parts.Reverse();
        return parts;
    }

    public WeightedDiGraph()
    {
        Vertices = new Dictionary<int, WeightedDiGraphVertex>();
    }

    /// <summary>
    /// Add a new vertex to this graph.
    /// Time complexity: O(1).
    /// </summary>
    public void AddVertex(int value)
    {
        var newVertex = new WeightedDiGraphVertex(value);

        Vertices.Add(value, newVertex);
    }

    /// <summary>
    /// Add a new edge to this graph.
    /// Time complexity: O(1).
    /// </summary>
    public void AddEdge(int source, int dest, int key)
    {
        if (!Vertices.ContainsKey(source)
            || !Vertices.ContainsKey(dest))
        {
            throw new Exception("Source or Destination Vertex is not in this graph.");
        }

        if (Vertices[source].OutEdges.ContainsKey(Vertices[dest])
            || Vertices[dest].InEdges.ContainsKey(Vertices[source]))
        {
            throw new Exception("Edge already exists.");
        }

        var edge = new DiEdge(Vertices[source], Vertices[dest], key);
        
        Vertices[source].OutEdges.Add(Vertices[dest], edge);
        Vertices[dest].InEdges.Add(Vertices[source], edge);
    }

    public WeightedDiGraphVertex GetVertex(int key)
    {
        return Vertices[key];
    }

    public IEnumerable<WeightedDiGraphVertex> VerticesAsEnumberable => Vertices.Values;
}


