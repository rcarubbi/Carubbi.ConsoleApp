# Carubbi.ConsoleApp

### Overview
Carubbi.ConsoleApp is a .NET library designed to facilitate the creation of console applications using a builder pattern similar to that used in WebApplication and HostApplications. The primary goal is to move the logic typically found in Program.cs into a class that can receive dependency injections through ServiceCollection, thereby enhancing the testability and maintainability of console applications.

### Features
- **Builder Pattern**: Structured approach for configuring and building console applications.
- **Dependency Injection**: Supports dependency injection via ServiceCollection, allowing for loosely coupled components and easier testing.
- **Enhanced Testability**: Separation of concerns by moving application logic into manageable classes.

### Getting Started
1. **Installation**
   - Install the `Carubbi.ConsoleApp` NuGet package in your project:
     ```
     Install-Package Carubbi.ConsoleApp
     ```

2. **Usage**
   - Define your console application logic in a dedicated class, following a builder pattern similar to setting up a web or host application.
   ```csharp
   public class DemoConsoleApp(Arguments arguments, ILoggerFactory loggerFactory, IDateTimeWrapper dateTimeWrapper, IClientA clientA, IClientB clientB) : IConsoleApp
   {
        public async Task<ExitCodes> RunAsync()
        {
            // your logic goes here
        }
   }
   ```
3. **Switch mapping**
   - You can map the arguments to the IConfiguration adding a dictionary to the ConsoleApplicationBuilder as shown in the next section. For easier consumption add properties to map the configuration to them. To acheive this, you can create an Arguments class as shown below: 
   
   ```csharp
    public class Arguments(IConfiguration configuration)
    {
        public static IDictionary<string, string>? SwitchMap { get; internal set; } = new Dictionary<string, string>()
        {
            {"--v", nameof(Version)}
        };

        public string Version { get; init; } = configuration![nameof(Version)]!;
    }
   ```
4. **Dependency Injection**
   - Register your console application class and services in Startup.cs or equivalent. The `args` argument is the built-in one provided for console apps in .net 
   ```csharp
   var builder = ConsoleApplication.CreateBuilder(args, Arguments.SwitchMap);
   builder.Services.AddConsoleApp<DemoConsoleApp>();
   ```

### Additional Information

For more detailed instructions and updates, visit the [This demo GitHub repository](https://github.com/rcarubbi/ConsoleAppWithResilientFlurlDemo).

Contributions and issues can be reported on the repository.

#### License
This library is licensed under the MIT License. See the LICENSE file for more details.
