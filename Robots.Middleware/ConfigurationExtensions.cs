using Microsoft.Extensions.Configuration;

namespace Robots.Middleware;

public static class ConfigurationExtensions
{
    public static string GetDelimiter(this IConfiguration configuration)
    {
        const string key = "InputDelimiter";
        var result = Tools.Configuration.GetValue<string>(key);
        // TODO Logger.Warning($"Can't read a key {key} from configuration file. Using default value ' '");
        return result ?? " ";
    }
}