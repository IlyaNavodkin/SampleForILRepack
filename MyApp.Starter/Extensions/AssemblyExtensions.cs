using System.Reflection;

namespace MyApp.Starter.Extensions;

public static class AssemblyExtensions
{
    public static string GetFriendlyNameWithVersion(this Assembly assembly)
    {
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        var assemblyName = assembly.GetName();

        return $"{assemblyName.Name} v{assemblyName.Version}";
    }
}


