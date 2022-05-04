namespace PlatformTest;

public class Bus
{
    public int Id { get; set; }
    public int Price { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeSpan RouteTime { get; set; }
    public List<BusStop> Stops { get; set; } = new();
}

public struct BusStop
{
    public int Id;

    public TimeSpan TimeBeforeNextStop;

    public TimeSpan TimeOfArrival;
}