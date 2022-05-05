using PlatformTest.Graph;
using PlatformTest.Models;
using PlatformTest.Strategies;

namespace PlatformTest.PathCalculation;

public class PathCalculator
{
    private readonly WeightedDiGraph _graph;

    private readonly Dictionary<int, Bus> _buses = new();

    public PathCalculator(List<Bus> buses)
    {
        _graph = InitializeGraph(buses);
    }

    public PathResult CalculatePriceBasedPath(int source, int destination)
    {
        var algorithm = new DijikstraAlgorithm();
        var strategy = new PriceBasedStrategy(_buses);
        
        _graph.SetStrategy(strategy);
        
        var result = algorithm.FindShortestPath(_graph, source);
        var path = _graph.GetPath(result, source, destination);
        
        return new PathResult()
        {
            Length = strategy.CalculateTotalCost(path),
            Path = TransformPath(path)
        };
    }
    
    public PathResult CalculateTimeBasedPath(int source, int destination, TimeOnly departureTime)
    {
        var algorithm = new DijikstraAlgorithm();
        var strategy = new TimeBasedStrategy(this._buses, departureTime);
        
        _graph.SetStrategy(strategy);
        
        var result = algorithm.FindShortestPath(_graph, source);
        var path = _graph.GetPath(result, source, destination);
        
        return new PathResult()
        {
            Length = strategy.CalculateTotalTime(path),
            Path = TransformPath(path)
        };
    }

    private Dictionary<int,int> TransformPath(List<IStrategy.PathPart> parts)
    {
        //Id остановки, Id Автобуса
        Dictionary<int, int> map = new ();

        for (int i = 1; i < parts.Count; i++)
        {
            map.Add(parts[i].SourceVertexId, _buses[parts[i - 1].EdgeId].Id);
        }

        map.Add(parts.Last().TargetVerexId, _buses[parts.Last().EdgeId].Id);
        
        return map;
    }
        
    
    private WeightedDiGraph InitializeGraph(List<Bus> buses)
    {
        var graph = new WeightedDiGraph();

        var busStops = buses.SelectMany(x => x.Stops).DistinctBy(x => x.Id);

        foreach (var bStop in busStops)
        {
            graph.AddVertex(bStop.Id);
        }

        int counter = 0;
        
        for (int i = 0; i < buses.Count; i++)
        {
            var bus = buses[i];
            
            for (int j = 0; j < bus.Stops.Count - 1; j++)
            {
                graph.AddEdge(bus.Stops[j].Id, bus.Stops[j + 1].Id, counter);
                _buses.Add(counter, bus);

                counter++;
            }
            
            graph.AddEdge(bus.Stops.Last().Id, bus.Stops.First().Id, counter);
            _buses.Add(counter, bus);
            
            counter++;
        }

        return graph;
    }
}