namespace PlatformTest.Strategies;

public interface IStrategy
{
    int CalculateParameter(List<PathPart> parts);

    struct PathPart
    {
       public int EdgeId { get; set; }

       public int SourceVertexId { get; set; }
       
       public int TargetVerexId { get; set; }
    }
}