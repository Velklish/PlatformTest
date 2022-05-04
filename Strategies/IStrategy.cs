namespace PlatformTest;

public interface IStrategy
{
    int CalculateParameter(CalculationContext context, DiEdge edge);
}