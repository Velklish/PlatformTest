namespace PlatformTest;

    /// <summary>
    /// A weighted graph implementation.
    /// IEnumerable enumerates all vertices.
    /// </summary>
    public class WeightedDiGraph
    {
        internal Dictionary<int, WeightedDiGraphVertex> Vertices { get; set; }

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
        public void AddEdge(int source, int dest, Bus performer, IStrategy strategy)
        {
            if (source == null || dest == null)
            {
                throw new ArgumentException();
            }

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

            Vertices[source].OutEdges.Add(Vertices[dest], new DiEdge(Vertices[source], Vertices[dest], performer, strategy));
            Vertices[dest].InEdges.Add(Vertices[source], new DiEdge(Vertices[source], Vertices[dest], performer, strategy));
        }

        public WeightedDiGraphVertex GetVertex(int key)
        {
            return Vertices[key];
        }
        
        public IEnumerable<WeightedDiGraphVertex> VerticesAsEnumberable => Vertices.Select(x => x.Value);
    }

public class WeightedDiGraphVertex
{
    public int Key { get; set; }

    public Dictionary<WeightedDiGraphVertex, DiEdge> OutEdges { get; }
    public Dictionary<WeightedDiGraphVertex, DiEdge> InEdges { get; }

    public IEnumerable<DiEdge> Edges => OutEdges.Values;

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


