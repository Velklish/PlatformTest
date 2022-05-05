namespace PlatformTest.Strategies;

class TimeBasedStrategy : IStrategy
{
    private readonly Dictionary<int, Bus> _buses;
    private readonly TimeOnly _departureTime;
    
    public TimeBasedStrategy(Dictionary<int, Bus> buses, TimeOnly departureTime)
    {
        _buses = buses;
        _departureTime = departureTime;
    }

    public int CalculateParameter(List<IStrategy.PathPart> parts)
    {
        return this.SimulatePath(parts).Last();
    }

    public int CalculateTotalTime(List<IStrategy.PathPart> parts)
    {
        return this.SimulatePath(parts).Sum();
    }

    private List<int> SimulatePath(List<IStrategy.PathPart> parts)
    {
        List<int> results = new List<int>();
        TimeOnly currentTime = _departureTime;

        int lastResult;
        
        foreach (var part in parts)
        {
            lastResult = 0;
            
            var performer = _buses[part.EdgeId];
            var currentStop = _buses[part.EdgeId].Stops.Find(x => x.Id == part.SourceVertexId);

            if (currentTime < performer.StartTime)
            {
                lastResult += (performer.StartTime - currentTime + currentStop.TimeOfArrival + currentStop.TimeBeforeNextStop).Minutes;
            }
            
            var diff = TimeSpan.FromTicks((currentTime - performer.StartTime).Ticks % performer.RouteTime.Ticks);

            if (currentStop.TimeOfArrival >= diff || (diff.Minutes == 0 && currentStop.TimeOfArrival == performer.RouteTime))
            {
                lastResult += (int)(currentStop.TimeOfArrival - diff + currentStop.TimeBeforeNextStop).TotalMinutes;
            }
            else
            {
                lastResult += (int)(performer.RouteTime - diff + currentStop.TimeOfArrival).TotalMinutes;
            }

            currentTime = currentTime.AddMinutes(lastResult);
            
            results.Add(lastResult);
        }

        return results;
    }
}