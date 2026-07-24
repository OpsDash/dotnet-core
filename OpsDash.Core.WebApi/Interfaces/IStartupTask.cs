namespace OpsDash.Core.WebApi.Interfaces;

public interface IStartupTask
{
    Task Execute(IServiceProvider services, CancellationToken ct);
}