using project.Data;
using project.Models;
using Microsoft.EntityFrameworkCore;


namespace project;

public class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }

    public static void Main(string[] args)
    {
        // CreateHostBuilder(args).Build().Run();
        
        var host = CreateHostBuilder(args).Build();

        // For auto run migration, comment out below
        // using (var scope = host.Services.CreateScope())
        // {
        //     var db = scope.ServiceProvider.GetRequiredService<PsqlDbContext>();
        //     db.Database.Migrate();
        // }

        host.Run();
    }

    // Below is old code from dotnet default webapi project.
    // not used anymore as moved the functionality to Startup.cs
    //
    // public static void MainOld(string[] args)
    // {
    //     var builder = WebApplication.CreateBuilder(args);
    //
    //     var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    //     if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    //     {
    //         // for dev server, get the db conn string from env or setting file.
    //         connectionString ??= builder.Configuration.GetConnectionString("DefaultConnection");
    //     }
    //     else
    //     {
    //         // if it is production, get the connection string from ENV VAR that Heroku supplies.
    //         connectionString = ConfigurationHelper.GetHerokuConnectionString();
    //     }
    //
    //     builder.Services.AddDbContext<PsqlDbContext>(options =>
    //         options.UseNpgsql(connectionString)
    //     );
    //
    //     // Add services to the container.
    //     builder.Services.AddControllers();
    //     // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //     builder.Services.AddEndpointsApiExplorer();
    //     builder.Services.AddSwaggerGen();
    //
    //     var app = builder.Build();
    //
    //
    //     // Configure the HTTP request pipeline.
    //     if (app.Environment.IsDevelopment())
    //     {
    //         app.UseSwagger();
    //         app.UseSwaggerUI();
    //     }
    //
    //     app.UseHttpsRedirection();
    //
    //     app.UseAuthorization();
    //
    //     app.MapControllers();
    //
    //     app.Run();
    // }
}