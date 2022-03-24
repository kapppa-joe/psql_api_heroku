using project.Data;
using project.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// for dev server, get the db conn string from env or setting file.
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
connectionString ??= builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PsqlDbContext>(options => 
    options.UseNpgsql(connectionString)
);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
