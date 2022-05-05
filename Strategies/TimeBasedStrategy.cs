using PlatformTest.Models;

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

        foreach (var part in parts)
        {
            int result = 0;
            
            var performer = _buses[part.EdgeId];
            var currentStop = _buses[part.EdgeId].Stops.Find(x => x.Id == part.SourceVertexId);

            // Если автобус еще не начал маршрут, то ждем начала + прибытия на остановку
            if (currentTime < performer.StartTime)
            {
                result += (int)(performer.StartTime - currentTime + currentStop.TimeOfArrival + currentStop.TimeBeforeNextStop).TotalMinutes;
            }
            else
            {
                //Текущая минута маршрута автобуса в его RouteTime
                var diff = TimeSpan.FromTicks((currentTime - performer.StartTime).Ticks % performer.RouteTime.Ticks);

                //Если текущее время больше, чем время прибытия на остановку, то ждем конца цикла + время прибытия
                if (currentStop.TimeOfArrival >= diff || (diff.Minutes == 0 && currentStop.TimeOfArrival == performer.RouteTime))
                {
                    result += (int)(currentStop.TimeOfArrival - diff + currentStop.TimeBeforeNextStop).TotalMinutes;
                }
                //Если текущее время меньше, то ждем прибытия
                else
                {
                    result += (int)(performer.RouteTime - diff + currentStop.TimeOfArrival).TotalMinutes;
                }
            }
            
            currentTime = currentTime.AddMinutes(result);
            
            results.Add(result);
        }

        return results;
    }
}