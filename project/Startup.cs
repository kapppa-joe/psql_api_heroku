using project.Data;
using Microsoft.EntityFrameworkCore;

namespace project;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        string connectionString = null;
        string dbConnectionStringHeroku = ConfigurationHelper.GetHerokuConnectionString();
        if (string.IsNullOrEmpty(dbConnectionStringHeroku))
        {
            // if cannot get db connection string for Heroku:
            // first, try to grab a connection string from env (for docker dev)
            connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            // if not found in env, assume it is in local dev environment and use the db conn string from config file.
            connectionString ??= Configuration.GetConnectionString("DefaultConnection");
        }
        else
        {
            // otherwise, just use the db connection string for Heroku.
            connectionString = dbConnectionStringHeroku;
        }
        
        
        services.AddDbContext<PsqlDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints((endpoints => { endpoints.MapControllers();}));

    }
}