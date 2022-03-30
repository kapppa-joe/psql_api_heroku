using System.Text.Json;

namespace unit_tests;
public static class Helper
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web) { };

    public static T? ParseJsonToObject<T>(string json)
    {
       return JsonSerializer.Deserialize<T>(json, Options);
    }
}