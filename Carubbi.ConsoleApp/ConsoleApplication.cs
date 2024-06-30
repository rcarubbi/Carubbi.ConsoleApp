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

    public async Task<ExitCodes> RunInParallelAsync()
    {
        var serviceProvider = _services.BuildServiceProvider();
        var consoleApps = serviceProvider.GetServices<IConsoleApp>();
        Task<ExitCodes>[] tasks = consoleApps.Select(c => c.RunAsync()).ToArray();

        var exitCodes = await Task.WhenAll(tasks);

        return EvaluateExitCodes(exitCodes);
    }

    public async Task<ExitCodes> RunSequenciallyAsync()
    {
        var serviceProvider = _services.BuildServiceProvider();
        var consoleApps = serviceProvider.GetServices<IConsoleApp>();
        var exitCodes = new List<ExitCodes>();

        foreach (var app in consoleApps)
        {
            exitCodes.Add(await app.RunAsync());
        }

        return EvaluateExitCodes(exitCodes);
    }

    public async Task<ExitCodes> RunAsync<T>() where T : class, IConsoleApp
    {
        var serviceProvider = _services.BuildServiceProvider();
        var consoleApp = serviceProvider.GetRequiredService<T>();
        return await consoleApp.RunAsync();
    }

    public async Task<ExitCodes> RunAsync()
    {
        var serviceProvider = _services.BuildServiceProvider();
        var consoleApp = serviceProvider.GetRequiredService<IConsoleApp>();
        return await consoleApp.RunAsync();
    }

    private static ExitCodes EvaluateExitCodes(IEnumerable<ExitCodes> exitCodes)
    {
        if (exitCodes.Any(x => x.HasFlag(ExitCodes.Error)))
        {
            return ExitCodes.Error;
        }
        else if (exitCodes.Any(x => x.HasFlag(ExitCodes.Warning)))
        {
            return ExitCodes.Warning;
        }
        else
        {
            return ExitCodes.Ok;
        }
    }

   
}
