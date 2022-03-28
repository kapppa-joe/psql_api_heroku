namespace project.Data;

public class ConfigurationHelper
{
    public static string GetHerokuConnectionString() {
        // Get the connection string from the ENV variables
        string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "";
        
        if (string.IsNullOrEmpty(connectionUrl)) {
            return "";
        }

        // parse the connection string
        var databaseUri = new Uri(connectionUrl);

        string db = databaseUri.LocalPath.TrimStart('/');
        string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

        return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
    }
}
