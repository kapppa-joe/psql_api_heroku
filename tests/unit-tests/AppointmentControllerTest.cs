using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using FluentAssertions;
using project;
using project.Data;
using project.Models;


namespace unit_tests;

public class TestAppointmentController
{
    private readonly HttpClient _httpClient;

    private PsqlDbContext _dbContext;

    public TestAppointmentController()
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // remove the dbcontext of actual postgres db and stub it with a in memory sqlite db
                    var descriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<PsqlDbContext>)
                    );

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<PsqlDbContext>(opt =>
                        opt.UseInMemoryDatabase("test_database")
                    );
                });
            }
        );
        _httpClient = factory.CreateClient();

        var options = new DbContextOptionsBuilder<PsqlDbContext>()
            .UseInMemoryDatabase(databaseName: "test_database")
            .Options;
        _dbContext = new PsqlDbContext(options);
    }
    
        [SetUp]
        public void Setup()
        {
            // clear the database before each test.
            _dbContext.Appointments.RemoveRange(_dbContext.Appointments);
            _dbContext.SaveChanges();
        }
    
        [Test]
        public async Task GetAppointment_responds_with_OK_and_empty_array()
        {
            var response = await _httpClient.GetAsync("/api/appointment");
            
            response.Should().Be200Ok();
            
            var contentJson = await response.Content.ReadAsStringAsync();
            
            contentJson.Should().BeEquivalentTo("[]");
        }
        
        [Test]
        public async Task Endpoint_in_plural_form_also_works()
        {
            var response = await _httpClient.GetAsync("/api/appointments");
            
            response.Should().Be200Ok();
            
            var contentJson = await response.Content.ReadAsStringAsync();
            
            contentJson.Should().BeEquivalentTo("[]");
        }
    
        [Test]
        public async Task GetAppointment_responds_with_an_array_of_notes()
        {
            // Arrange
            var testAppointment = new Appointment()
            {
                id = 1, 
                Name = "Al Parker", 
                Email = "alpaca@llama.com",
                Date = "25th Apr Monday",
                Type = "Consultation"
            };
            _dbContext.Appointments.Add(testAppointment);
            await _dbContext.SaveChangesAsync();
        
            var expected = new[] {testAppointment};
        
            // Act
            var response = await _httpClient.GetAsync("/api/appointment");
            var contentJson = await response.Content.ReadAsStringAsync();
            var responseBody = Helper.ParseJsonToObject<Appointment[]>(contentJson);
        
            // Assert
            response.Should().Be200Ok();
            responseBody.Should().NotBeNull();
            responseBody.Should().BeEquivalentTo(expected);
        }

        // [Test]
        // public async Task PostAppointment_respond_with_201()
        // {
        //     // Arrange
        //     var reqBody = "{"name": }";
        // }
}