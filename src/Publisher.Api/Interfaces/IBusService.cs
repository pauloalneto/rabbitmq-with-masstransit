namespace Publisher.Api.Interfaces;

public interface IBusService
{
    Task<bool> PublishAsync<T>(T message, string routerKey, CancellationToken cancellationToken = default);
}