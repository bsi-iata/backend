using Dapper.FluentMap.Mapping;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Repository.Mapping;

public class PackageMap : EntityMap<Package>
{
    public PackageMap()
    {
        Map(p => p.Order).Ignore();
        Map(p => p.Contact).Ignore();
        Map(p => p.Goods).Ignore();
    }
}