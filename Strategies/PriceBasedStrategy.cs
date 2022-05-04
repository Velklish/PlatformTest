namespace PlatformTest.Strategies;

class PriceBasedStrategy : IStrategy
{
    public int CalculateParameter(CalculationContext context, DiEdge edge)
    {
        if (context.CurrentPath.Count > 1)
        {
            if (context.CurrentPath.Last().Performer == edge.Performer)
            {
                return 0;
            }

            return edge.Performer.Price;
        }

        return edge.Performer.Price;
    }
}