using System.Text.Json;

namespace unit_tests;
public class Helper
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web) { };

    public static T? ParseJsonToObject<T>(string json)
    {
       return JsonSerializer.Deserialize<T>(json, Options);
    }
}