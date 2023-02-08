using System.Reflection;
using Microsoft.Extensions.Configuration;
using Logging.Net;

namespace Robots.Middleware;

public static class Tools
{
    public static readonly IConfiguration Configuration;
    
    static Tools()
    {
        var path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
                
        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false);
        Configuration = builder.Build();
        
        Logger.LogToFile(Path.Join(
            path,
            Configuration.GetValue<string>("Logs"),
            DateTime.UtcNow.ToString("yy-MM-dd"), ".log"));
    }
}