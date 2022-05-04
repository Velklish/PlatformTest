using Advanced.Algorithms.DataStructures;
using PlatformTest.Graph;

namespace PlatformTest;

/// <summary>
/// A dijikstra algorithm implementation using Fibonacci Heap.
/// </summary>
public class DijikstraAlgorithm
{
    /// <summary>
    /// Get shortest distance to target.
    /// </summary>
    public Dictionary<int, int> FindShortestPath(
        WeightedDiGraph graph,
        int source)
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

        //until heap is empty
        while (minHeap.Count > 0)
        {
            //next min vertex to visit
            current = minHeap.Extract();
            heapMapping.Remove(current.Vertex);

            //no path exists, so return max value
            if (current.Distance.Equals(int.MaxValue))
            {
                return null;
            }

            //visit neighbours of current
            foreach (var neighbour in graph.GetVertex(current.Vertex).OutEdges.Keys)
            {
                var edgeWeight = graph.EdgeWeight(parentMap, source, current.Vertex, neighbour.Key);

                //new distance to neighbour
                var newDistance = current.Distance + edgeWeight;

                //current distance to neighbour
                var existingDistance = progress[neighbour.Key];

                //update distance if new is better
                if (newDistance.CompareTo(existingDistance) < 0)
                {
                    progress[neighbour.Key] = newDistance;

                    if (!heapMapping.ContainsKey(neighbour.Key))
                    {
                        var wrap = new MinHeapWrap() { Distance = newDistance, Vertex = neighbour.Key };
                        minHeap.Insert(wrap);
                        heapMapping.Add(neighbour.Key, wrap);
                    }
                    else
                    {
                        //decrement distance to neighbour in heap
                        var decremented = new MinHeapWrap()
                        {
                            Distance = newDistance,
                            Vertex = neighbour.Key
                        };

                        minHeap.UpdateKey(heapMapping[neighbour.Key], decremented);
                        heapMapping[neighbour.Key] = decremented;
                    }

                    //trace parent
                    parentMap[neighbour.Key] = current.Vertex;
                }
            }

        }

        return parentMap;
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
}