using Carubbi.ConsoleApp.Enums;
using Carubbi.ConsoleApp.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carubbi.ConsoleApp;

public class ConsoleApplication
{
    private readonly IServiceCollection _services;
    internal ConsoleApplication(IServiceCollection services)
    {
        _services = services;
    }

    private const string DOTNET_ENVIRONMENT = "DOTNET_ENVIRONMENT";
    private const string APP_SETTINGS = "appSettings.json";
    private const string APP_SETTINGS_ENVIRONMENT = "appSettings.{0}.json";

    public static ConsoleApplicationBuilder CreateBuilder(string[] arguments, IDictionary<string, string>? switchMap = null)
    {
        var configurationBuilder = new ConfigurationBuilder();

        var environmentAppSettingsFileName = string.Format(APP_SETTINGS_ENVIRONMENT, Environment.GetEnvironmentVariable(DOTNET_ENVIRONMENT));

        var configuration = configurationBuilder
          .AddJsonFile(APP_SETTINGS, optional: true, reloadOnChange: true)
          .AddJsonFile(environmentAppSettingsFileName, optional: true, reloadOnChange: true)
          .AddCommandLine(arguments, switchMap)
          .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        var builder = new ConsoleApplicationBuilder(services, configuration);
      

        return builder;
    }

    public async Task<ExitCodes> RunAsync()
    {
        var serviceProvider = _services.BuildServiceProvider();
        var consoleApp = serviceProvider.GetRequiredService<IConsoleApp>();
        return await consoleApp.RunAsync();
    }
}
