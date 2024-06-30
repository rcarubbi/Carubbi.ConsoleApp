using Carubbi.ConsoleApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Carubbi.ConsoleApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsoleApp<T>(this IServiceCollection services) where T : class, IConsoleApp
        {
            return services.AddSingleton<T>()
                .AddSingleton<IConsoleApp>(sp => sp.GetRequiredService<T>());

        }
    }
}
