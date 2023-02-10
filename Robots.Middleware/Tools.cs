using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Robots.Middleware;

public static class Tools
{
    public static readonly IConfiguration Configuration;
    private const string AppConfigName = "appsettings.json";
    static Tools()
    {
        var assemblyLocation = Assembly.GetCallingAssembly().Location;
        var path = Path.GetDirectoryName(assemblyLocation);
        if (string.IsNullOrEmpty(path))
            throw new ApplicationException($"Can't get a path to assembly {assemblyLocation}");
                
        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile(AppConfigName, optional: false);
        Configuration = builder.Build();
    }
}