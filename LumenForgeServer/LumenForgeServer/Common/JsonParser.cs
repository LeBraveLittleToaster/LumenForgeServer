using System.Text.Json;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace LumenForgeServer.Common;

/// <summary>
/// Helper for creating shared JSON serializer options used across the application.
/// </summary>
public static class Json
{
    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions();
        options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        return options;
    }
    
}
