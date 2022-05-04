namespace PlatformTest.Strategies;

class PriceBasedStrategy : IStrategy
{
    private Dictionary<int, Bus> _buses;

    public PriceBasedStrategy(Dictionary<int, Bus> buses)
    {
        _buses = buses;
    }
    
    public int CalculateParameter(List<IStrategy.PathPart> parts)
    {
        if (parts.Count > 1)
        {
            if (_buses[parts.Last().EdgeId] == _buses[parts.First().EdgeId] &&
                parts.First().SourceVertexId == parts.Last().TargetVerexId)
            {
                return _buses[parts.Last().EdgeId].Price;
            }
            
            if (_buses[parts.Last().EdgeId] == _buses[parts[^2].EdgeId])
            {
                return 0;
            }
            
            return _buses[parts.Last().EdgeId].Price;
        }

        return _buses[parts.First().EdgeId].Price;
    }

    public int CalculateTotalCost(List<IStrategy.PathPart> parts)
    {
        var prevPart = parts.First().EdgeId;
        var bus = _buses[prevPart];
        int cost = bus.Price;
        
        foreach (var part in parts)
        {
            if (_buses[part.EdgeId] != bus)
            {
                cost += _buses[part.EdgeId].Price;
            }

            bus = _buses[part.EdgeId];
        }

        return cost;
    }
}