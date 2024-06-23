using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carubbi.ConsoleApp;

public class ConsoleApplicationBuilder(IServiceCollection services, IConfiguration configuration)
{
     
    public ConsoleApplication Build()
    {
        var application = new ConsoleApplication(services);

        return application;
    }

    public IServiceCollection Services => services;

    public IConfiguration Configuration => configuration;
}