using PlatformTest;
using PlatformTest.Strategies;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Название файла - ");
        string path = @"..\..\..\" + Console.ReadLine();
        
        Console.Write("Начальная остановка - ");
        int source = Int32.Parse(Console.ReadLine());
        
        Console.Write("Конечная остановка - ");
        int destination = Int32.Parse(Console.ReadLine());
        
        Console.Write("Время отправления - ");
        var departureTime = TimeOnly.FromDateTime(DateTime.Parse(Console.ReadLine()));
        
        var file = new StreamReader(path);
        int busCount = Int32.Parse(file.ReadLine());
        int stopsCount = Int32.Parse(file.ReadLine());
        var buses = new List<Bus>(busCount);
        var lines = file.ReadToEnd().Split(Environment.NewLine);
        var startTimes = lines[0].Split(' ');
        var prices = lines[1].Split(' ');

        for (int i = 0; i < busCount; i++)
        {
            var bus = new Bus();
            bus.Id = i + 1;
            bus.Price = Int32.Parse(prices[i]);
            bus.StartTime = TimeOnly.Parse(startTimes[i]);

            var busStops = lines[i + 2].Split(' ');
            var busStopsCount = Int16.Parse(lines[i + 2].Split(' ').First());
            busStops = busStops.Skip(1).ToArray();

            TimeSpan fullPeriod = new();
            
            for (int j = 0; j < busStopsCount; j++)
            {
                var bStop = new BusStop
                {
                    Id = Int32.Parse(busStops[j]),
                    TimeBeforeNextStop = new TimeSpan(0, Int16.Parse(busStops[j + busStopsCount]), 0),
                    TimeOfArrival = fullPeriod
                };
                
                bus.Stops.Add(bStop);
                fullPeriod += bStop.TimeBeforeNextStop;
            }

            bus.RouteTime = fullPeriod;
            buses.Add(bus);
        }

        var calculator = new PathCalculator(new TimeBasedStrategy(), buses);
        
        var result = calculator.CalculatePath(source, destination, departureTime);
        Console.WriteLine("Время самого быстрого пути " + result.Length);
        PrintPath(result.Path);
        
        calculator.SetStrategy(new PriceBasedStrategy());

        result = calculator.CalculatePath(source, destination, departureTime);
        Console.WriteLine("Цена самого дешевого пути " + result.Length);
        PrintPath(result.Path);
    }
    
    private static void PrintPath(List<ShortestPathResult.PathResult> path)
    {
        Console.WriteLine("Путь: ");

        foreach (var vertex in path)
        {
            Console.WriteLine(vertex.VertexId + " на автобусе номер " + vertex.Performer.Id);
        }
    }
}




