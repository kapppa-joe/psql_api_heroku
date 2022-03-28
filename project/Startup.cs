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
        // try grab the db connection string for Heroku first.
        string connectionString = ConfigurationHelper.GetHerokuConnectionString();
        
        // if failed, try to grab a connection string from env (for docker dev)
        if (string.IsNullOrEmpty(connectionString))
        {
            // if not found in env, assume it is in local dev environment and use the db conn string from config file.
            connectionString = 
                Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
                    ?? Configuration.GetConnectionString("DefaultConnection");
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