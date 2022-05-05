using PlatformTest;

class Program
{
    static void Main(string[] args)
    {
        /*Console.Write("Название файла - ");
        string path = @"..\..\..\" + Console.ReadLine();
        
        Console.Write("Начальная остановка - ");
        int source = Int32.Parse(Console.ReadLine());
        
        Console.Write("Конечная остановка - ");
        int destination = Int32.Parse(Console.ReadLine());
        
        Console.Write("Время отправления - ");
        var departureTime = TimeOnly.FromDateTime(DateTime.Parse(Console.ReadLine()));*/

        string path = @"..\..\..\" + "data.txt";
        int source = 3;
        int destination = 5;
        var departureTime = new TimeOnly(14, 0, 0);
        
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

        var calculator = new PathCalculator( buses);
        
        var result = calculator.CalculatePriceBasedPath(source, destination);
        result.ForEach(Console.Write);
        Console.WriteLine();

        result = calculator.CalculateTimeBasedPath(source, destination, departureTime);
        result.ForEach(Console.Write);
    }
}




