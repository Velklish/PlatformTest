namespace PlatformTest.PathCalculation;

public class PathResult
{
    /// <summary> Путь в формате Id остановки, Id Автобуса. </summary>
    public Dictionary<int, int> Path { get; init; } = new(); 
    
    /// <summary> Затраты на путь. </summary>
    public int Length { get; init;  }
}