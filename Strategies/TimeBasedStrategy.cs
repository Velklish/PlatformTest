namespace PlatformTest.Strategies;

class TimeBasedStrategy : IStrategy
{
    public int CalculateParameter(CalculationContext context, DiEdge edge)
    {
        var performer = edge.Performer;
        var currentStop = performer.Stops.First(x => x.Id == edge.SourceVertex.Key);
        var targetStop = performer.Stops.First(x => x.Id == edge.TargetVertexKey);

        int result = 0;
        
        if (context.CurrentTime < performer.StartTime)
        {
            result += (performer.StartTime - context.CurrentTime).Minutes;
        }

        var diff = TimeSpan.FromTicks((context.CurrentTime - performer.StartTime).Ticks % performer.RouteTime.Ticks);

        if (currentStop.TimeOfArrival >= diff || (diff.Minutes == 0 && currentStop.TimeOfArrival == performer.RouteTime))
        {
            result += (currentStop.TimeOfArrival - diff + currentStop.TimeBeforeNextStop).Minutes;
        }
        else
        {
            result += (performer.RouteTime - diff + currentStop.TimeOfArrival).Minutes;
        }

        return result;
    }
}