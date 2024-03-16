using Dapper.FluentMap;
using GotoFreight.IATA.Repository.Mapping;

namespace GotoFreight.IATA;

public class Bootstrap : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        //init dapper mapping
        FluentMapper.EntityMaps.Clear();
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new OrderMap());
            config.AddMap(new PackageMap());
            config.AddMap(new GoodMap());
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}