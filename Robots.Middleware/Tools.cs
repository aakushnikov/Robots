using System.Reflection;
using Microsoft.Extensions.Configuration;
using Logging.Net;

namespace Robots.Middleware;

public static class Tools
{
    public static readonly IConfiguration Configuration;
    private const string appConfigName = "appsettings.json";
    
    static Tools()
    {
        var assemblyLocation = Assembly.GetCallingAssembly().Location;
        var path = Path.GetDirectoryName(assemblyLocation);
        if (string.IsNullOrEmpty(path))
            throw new ApplicationException($"Can't get a path to assembly {assemblyLocation}");
                
        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile(appConfigName, optional: false);
        Configuration = builder.Build();
        
        Logger.LogToFile(Path.Join(
            path,
            Configuration.GetValue<string>("Logs"),
            DateTime.UtcNow.ToString("yy-MM-dd"), ".log"));
    }
}