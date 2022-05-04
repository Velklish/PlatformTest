namespace PlatformTest;

public class CalculationContext
{
    public TimeOnly CurrentTime { get; private set; }

    public List<ShortestPathResult.PathResult> CurrentPath { get; private set; } = new();

    public CalculationContext(TimeOnly currentTime)
    {
        CurrentTime = currentTime;
    }
    
    public void UpdateContext(TimeOnly newTime, ShortestPathResult.PathResult newPath)
    {
        CurrentTime = newTime;
        AddToPath(newPath);
    }

    public void AddToPath(ShortestPathResult.PathResult result)
    {
        CurrentPath.Add(result);
    }
}