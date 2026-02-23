using System.Text.Json;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace LumenForgeServer.Common;

public static class Json
{
    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions();
        options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        return options;
    }
    
}