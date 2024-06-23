using Carubbi.ConsoleApp.Enums;

namespace Carubbi.ConsoleApp.Extensions
{
    public static class EnvironmentExtensions
    {
        public static void Exit(ExitCodes exitCode)
        {
            Environment.Exit((int)exitCode);
        }
    }
}
