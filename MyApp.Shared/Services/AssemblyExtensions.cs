using System.Reflection;

namespace MyApp.Shared.Services;

public static class AssemblyExtensions
{
    public static string GetFriendlyNameWithVersion(this Assembly assembly)
    {
        var assemblyName = assembly.GetName();
        return $"{assemblyName.Name} v{assemblyName.Version}";
    }
}