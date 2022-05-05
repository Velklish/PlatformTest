namespace PlatformTest.Models;

public class Bus
{
    public int Id { get; set; }
    
    public int Price { get; set; }
    
    public TimeOnly StartTime { get; set; }
    
    /// <summary> Время полного маршрута автобуса. </summary>
    public TimeSpan RouteTime { get; set; }
    
    public List<BusStop> Stops { get; set; } = new();
}

public class BusStop
{
    public int Id;

    /// <summary> Время до следующей остановки. </summary>
    public TimeSpan TimeBeforeNextStop;

    /// <summary> Время прибытия в рамках полного маршрута автобуса. </summary>
    public TimeSpan TimeOfArrival;
}