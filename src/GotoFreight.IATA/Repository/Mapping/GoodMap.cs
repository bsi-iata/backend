using Dapper.FluentMap.Mapping;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Repository.Mapping;

public class GoodMap : EntityMap<Good>
{
    public GoodMap()
    {
        Map(p => p.Package).Ignore();
    }
}