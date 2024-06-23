using Carubbi.ConsoleApp.Enums;

namespace Carubbi.ConsoleApp.Interfaces;

public interface IConsoleApp
{
    Task<ExitCodes> RunAsync();
}
