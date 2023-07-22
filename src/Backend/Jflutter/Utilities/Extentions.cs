using System.Text.Json;

namespace Jflutter.Utilities;

public static class Extentions
{
    private  static JsonSerializerOptions? serializeOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = null,
        WriteIndented = true
    };
    public  static string AsJson (this object? obj)
    {
   
        return System.Text.Json.JsonSerializer.Serialize(obj, serializeOptions);
    }
}