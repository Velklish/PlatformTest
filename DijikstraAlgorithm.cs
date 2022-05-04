using Advanced.Algorithms.DataStructures;

namespace PlatformTest;

/// <summary>
/// A dijikstra algorithm implementation using Fibonacci Heap.
/// </summary>
public class DijikstraAlgorithm
{
    /// <summary>
    /// Get shortest distance to target.
    /// </summary>
    public ShortestPathResult FindShortestPath(
        WeightedDiGraph graph, 
        int source, 
        int destination,
        TimeOnly departureTime)
    {
        //track progress for distance to each Vertex from source
        var progress = new Dictionary<int, int>();

        //trace our current path by mapping current vertex to its Parent
        var parentMap = new Dictionary<int, int>();

        //min heap to pick next closest vertex 
        var minHeap = new FibonacciHeap<MinHeapWrap>();

        //keep references of heap Node for decrement key operation
        var heapMapping = new Dictionary<int, MinHeapWrap>();

        //add vertices to min heap and progress map
        foreach (var vertex in graph.VerticesAsEnumberable)
        {
            //init parent
            parentMap.Add(vertex.Key, default);

            //init to max value
            progress.Add(vertex.Key, int.MaxValue);
        }

        //start from source vertex as current 
        var current = new MinHeapWrap()
        {
            Distance = 0,
            Vertex = source
        };

        minHeap.Insert(current);
        heapMapping.Add(current.Vertex, current);
        
        TimeOnly currentTime = departureTime;
        CalculationContext context = new CalculationContext(currentTime);

        //until heap is empty
        while (minHeap.Count > 0)
        {
            //next min vertex to visit
            current = minHeap.Extract();
            heapMapping.Remove(current.Vertex);

            //no path exists, so return max value
            if (current.Distance.Equals(int.MaxValue))
            {
                return new ShortestPathResult(null, int.MaxValue);
            }

            //visit neighbours of current
            foreach (var neighbour in graph.GetVertex(current.Vertex).Edges
                         .Where(x => !x.TargetVertexKey.Equals(source)))
            {
                
                var edge = graph.GetVertex(current.Vertex)
                    .GetEdge(neighbour.TargetVertex);
                
                var edgeWeight = edge.Weight(context);

                //new distance to neighbour
                var newDistance = current.Distance + edgeWeight;

                //current distance to neighbour
                var existingDistance = progress[neighbour.TargetVertexKey];

                //update distance if new is better
                if (newDistance.CompareTo(existingDistance) < 0)
                {
                    progress[neighbour.TargetVertexKey] = newDistance;

                    if (!heapMapping.ContainsKey(neighbour.TargetVertexKey))
                    {
                        var wrap = new MinHeapWrap() { Distance = newDistance, Vertex = neighbour.TargetVertexKey };
                        minHeap.Insert(wrap);
                        heapMapping.Add(neighbour.TargetVertexKey, wrap);
                    }
                    else
                    {
                        //decrement distance to neighbour in heap
                        var decremented = new MinHeapWrap()
                        {
                            Distance = newDistance, 
                            Vertex = neighbour.TargetVertexKey
                        };
                        
                        minHeap.UpdateKey(heapMapping[neighbour.TargetVertexKey], decremented);
                        heapMapping[neighbour.TargetVertexKey] = decremented;
                    }

                    //trace parent
                    parentMap[neighbour.TargetVertexKey] = current.Vertex;
                    context.UpdateContext(currentTime.AddMinutes(edgeWeight), new ShortestPathResult.PathResult()
                    {
                        Performer = edge.Performer,
                        VertexId = current.Vertex
                    });
                }
            }

        }

        return TracePath(graph, parentMap, source, destination, departureTime);

    }

    /// <summary>
    /// Trace back path from destination to source using parent map.
    /// </summary>
    private ShortestPathResult TracePath(WeightedDiGraph graph, Dictionary<int, int> parentMap, int source,
        int destination, TimeOnly departureTime)
    {
        //trace the path
        var pathStack = new Stack<int>();

        pathStack.Push(destination);

        var currentV = destination;
        while (!Equals(currentV, default(int)) && !Equals(parentMap[currentV], default(int)))
        {
            pathStack.Push(parentMap[currentV]);
            currentV = parentMap[currentV];
        }

        //return result
        var resultVertices = new List<int>();
        var resultLength = 0;
        while (pathStack.Count > 0)
        {
            resultVertices.Add(pathStack.Pop());
        }

        var currentTime = departureTime;
        CalculationContext context = new CalculationContext(currentTime);

        var resultPath = new List<ShortestPathResult.PathResult>();

        for (int i = 0; i < resultVertices.Count - 1; i++)
        {
            var edge = graph.GetVertex(resultVertices[i])
                .GetEdge(graph.GetVertex(resultVertices[i + 1]));

            var edgeWeight = edge.Weight(context);
            currentTime.AddMinutes(edgeWeight);

            resultLength = resultLength + edgeWeight;
            
            resultPath.Add(new ShortestPathResult.PathResult()
            {
                Performer = edge.Performer,
                VertexId = resultVertices[i]
            });
        }
        
        var edg = graph.GetVertex(resultVertices[resultVertices.Count - 2])
            .GetEdge(graph.GetVertex(resultVertices[resultVertices.Count - 1]));
        
        resultPath.Add(new ShortestPathResult.PathResult()
        {
            Performer = edg.Performer,
            VertexId = resultVertices[resultVertices.Count - 1]
        });

        return new ShortestPathResult(resultPath, resultLength);
    }
}

/// <summary>
    /// Shortest path result object.
    /// </summary>
    public class ShortestPathResult
    {
        public ShortestPathResult(List<PathResult> path, int length)
        {
            Length = length;
            Path = path;
        }
        public int Length { get; internal set; }
        public List<PathResult> Path { get; private set; }

        public struct PathResult
        {
            public int VertexId;

            public Bus Performer;
        }
    }

    /// <summary>
    /// For fibornacci heap node.
    /// </summary>
    internal class MinHeapWrap : IComparable
    {
        internal int Vertex { get; set; }
        internal int Distance { get; set; }

        public int CompareTo(object obj)
        {
            return Distance.CompareTo((obj as MinHeapWrap).Distance);
        }
    }