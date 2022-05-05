using PlatformTest.Models;
using PlatformTest.PathCalculation;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Название файла - ");
        string path = @"..\..\..\" + Console.ReadLine();
        
        Console.Write("Начальная остановка - ");
        int source = Int32.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
        
        Console.Write("Конечная остановка - ");
        int destination = Int32.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
        
        Console.Write("Время отправления - ");
        var departureTime = TimeOnly.FromDateTime(DateTime.Parse(Console.ReadLine() ?? throw new InvalidOperationException()));

        var buses = ParseData(path);

        var calculator = new PathCalculator( buses);
        
        Console.WriteLine("Стратегия минимальной стоимости проезда: ");
        var result = calculator.CalculatePriceBasedPath(source, destination);
        PrintResult(result);

        Console.WriteLine();
        
        Console.WriteLine("Стратегия минимального времени проезда: ");
        result = calculator.CalculateTimeBasedPath(source, destination, departureTime);
        PrintResult(result);
    }
    
    private static void PrintResult(PathResult path)
    {
        Console.WriteLine("Затрачено: " + path.Length);
        Console.WriteLine("Путь: ");

        foreach (var part in path.Path)
        {
            Console.WriteLine("До остановки номер " + part.Key + " на автобусе номер " + part.Value);
        }
    }

    private static List<Bus> ParseData(string fileName)
    {
        var file = new StreamReader(fileName);
        
        int busCount = Int32.Parse(file.ReadLine() ?? throw new InvalidOperationException());
        int stopsCount = Int32.Parse(file.ReadLine() ?? throw new InvalidOperationException());
        
        var lines = file.ReadToEnd().Split(Environment.NewLine);
        
        var startTimes = lines[0].Split(' ');
        var prices = lines[1].Split(' ');
        
        var buses = new List<Bus>(busCount);
        
        for (int i = 0; i < busCount; i++)
        {
            var bus = new Bus();
            bus.Id = i + 1;
            bus.Price = Int32.Parse(prices[i]);
            bus.StartTime = TimeOnly.Parse(startTimes[i]);

            var busStops = lines[i + 2].Split(' ');
            var busStopsCount = int.Parse(busStops.First());
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

        return buses;
    }
}




