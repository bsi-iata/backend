using Dapper.FluentMap.Mapping;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Repository.Mapping;

public class OrderMap : EntityMap<Order>
{
    public OrderMap()
    {
        Map(p => p.Packages).Ignore();
    }
}