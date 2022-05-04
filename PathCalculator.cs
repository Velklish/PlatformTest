using PlatformTest.Graph;
using PlatformTest.Strategies;

namespace PlatformTest;

public class PathCalculator
{
    private WeightedDiGraph _graph;
    public Dictionary<int, Bus> Buses = new();
    //public Dictionary<int, BusStop> BusStops = new();

    public PathCalculator(List<Bus> buses)
    {
        _graph = this.InitializeGraph(buses);
    }

    public List<int> CalculatePriceBasedPath(int source, int destination)
    {
        var algorithm = new DijikstraAlgorithm();

        var strategy = new PriceBasedStrategy(this.Buses);
        _graph.CalculateStrategy = strategy;
        var result = algorithm.FindShortestPath(_graph, source);
        var path = _graph.GetPath(result, source, destination);
        List<int> pathResult = path.Select(x => x.SourceVertexId).ToList();
        pathResult.Add(path.Last().TargetVerexId);
        Console.WriteLine(strategy.CalculateTotalCost(path));
        return pathResult;
    }
    
    /*public ShortestPathResult CalculateTimeBasedPath(int source, int destination, TimeOnly departureTime)
    {
        var algorithm = new DijikstraAlgorithm();

        var result = algorithm.FindShortestPath(_graph, source, destination);
        return result;
    }*/
    
    private WeightedDiGraph InitializeGraph(List<Bus> buses)
    {
        var graph = new WeightedDiGraph();

        var busStops = buses.SelectMany(x => x.Stops).DistinctBy(x => x.Id);

        foreach (var bStop in busStops)
        {
            graph.AddVertex(bStop.Id);
            //BusStops.Add(bStop.Id, bStop);
        }

        int counter = 0;
        
        for (int i = 0; i < buses.Count; i++)
        {
            var bus = buses[i];
            
            for (int j = 0; j < bus.Stops.Count - 1; j++)
            {
                graph.AddEdge(bus.Stops[j].Id, bus.Stops[j + 1].Id, counter);
                this.Buses.Add(counter, bus);

                counter++;
            }
            
            graph.AddEdge(bus.Stops.Last().Id, bus.Stops.First().Id, counter);
            this.Buses.Add(counter, bus);
            
            counter++;
        }

        return graph;
    }
}