using Microsoft.Extensions.Configuration;

namespace Robots.Middleware;

public static class ConfigurationExtensions
{
    public static string GetDelimiter(this IConfiguration configuration)
    {
        return Tools.Configuration.GetValue<string>("InputDelimiter");
    }
}