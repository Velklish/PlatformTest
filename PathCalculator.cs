namespace PlatformTest;

public class PathCalculator
{
    public IStrategy Strategy;
    public List<Bus> Buses;

    public PathCalculator(IStrategy strategy, List<Bus> buses)
    {
        Strategy = strategy;
        Buses = buses;
    }

    public void SetStrategy(IStrategy strategy)
    {
        Strategy = strategy;
    }

    public ShortestPathResult CalculatePath(int source, int destination, TimeOnly departureTime)
    {
        var wGraph = InitializeGraph();
        var algorithm = new DijikstraAlgorithm();

        var result = algorithm.FindShortestPath(wGraph, source, destination, departureTime);
        return result;
    }
    
    private WeightedDiGraph InitializeGraph()
    {
        var graph = new WeightedDiGraph();
        var vertexes = Buses.SelectMany(x => x.Stops.Select(x => x.Id)).Distinct().ToList();
        vertexes.ForEach(graph.AddVertex);
        
        foreach (var bus in Buses)
        {
            for (int i = 0; i < bus.Stops.Count - 1; i++)
            {
                graph.AddEdge(bus.Stops[i].Id, bus.Stops[i + 1].Id, bus, Strategy);
            }

            graph.AddEdge(bus.Stops.Last().Id, bus.Stops.First().Id, bus, Strategy);
        }

        return graph;
    }
}