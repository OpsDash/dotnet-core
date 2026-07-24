using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpsDash.Core.WebApi.Interfaces;

namespace OpsDash.Core.WebApi.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddStartupMethod(
        this IServiceCollection services,
        Func<IServiceProvider, CancellationToken, Task> execute)
    {
        services.AddSingleton<IStartupTask>(new DelegateStartupTask(execute));
        return services;
    }

    public static async Task RunStartupTasks(this WebApplication app)
    {
        var tasks = app.Services.GetServices<IStartupTask>();

        foreach (var task in tasks)
        {
            await task.Execute(app.Services, app.Lifetime.ApplicationStopping);
        }
    }
    
    private sealed class DelegateStartupTask(Func<IServiceProvider, CancellationToken, Task> execute) : IStartupTask
    {
        public Task Execute(IServiceProvider services, CancellationToken ct)
            => execute(services, ct);
    }
}