using Logging.Net;
using Microsoft.Extensions.Configuration;

namespace Robots.Middleware;

public static class ConfigurationExtensions
{
    public static string GetDelimiter(this IConfiguration configuration)
    {
        const string key = "InputDelimiter";
        var result = Tools.Configuration.GetValue<string>(key);
        if (string.IsNullOrEmpty(result))
            Logger.Warn($"Can't read a key {key} from configuration file. Using default value ' '");
        return result ?? " ";
    }
}