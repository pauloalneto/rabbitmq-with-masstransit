using MassTransit;
using Publisher.Api.Interfaces;

namespace Publisher.Api.Services;

public class BusService : IBusService
{
    private readonly IBus _publishEndpoint;

    public BusService(IBus publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> PublishAsync<T>(T message, string routerKey, CancellationToken cancellationToken = default)
    {
        try
        {
            await _publishEndpoint.Publish(message, context =>
            {
                context.SetRoutingKey(routerKey);
            } ,cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
}