using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using FluentAssertions;
using Newtonsoft.Json.Linq;
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
    public async Task Responds_with_404_for_non_exist_endpoint()
    {
        var response = await _httpClient.GetAsync("/api/not_exist_end_point");
        response.Should().Be404NotFound();
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
    public async Task GetAppointment_responds_with_an_array_of_appointments()
    {
        // Arrange
        var testAppointment = new Appointment()
        {
            Name = "Al Parker",
            Email = "alpaca@llama.com",
            Date = "25th Apr Monday",
            Type = "Consultation"
        };
        var testAppointment2 = new Appointment()
        {
            Name = "Kyle",
            Email = "kyle@llama.com",
            Date = "26th Apr Tuesday",
            Type = "Training"
        };
        _dbContext.Appointments.Add(testAppointment);
        _dbContext.Appointments.Add(testAppointment2);
        await _dbContext.SaveChangesAsync();

        var expected = new[] { testAppointment, testAppointment2 };

        // Act
        var response = await _httpClient.GetAsync("/api/appointment");
        var contentJson = await response.Content.ReadAsStringAsync();
        var responseBody = Helper.ParseJsonToObject<Appointment[]>(contentJson);

        // Assert
        response.Should().Be200Ok();
        response.Should().BeAs(expected);
    }

    [Test]
    public async Task PostAppointment_respond_with_201_and_new_data()
    {
        // Arrange
        var testAppointment = new
        {
            Name = "Kyle",
            Email = "alpaca@llama.com",
            Date = "25th Apr Monday",
            Type = "Consultation"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/appointment", testAppointment);

        // Assert
        response.Should().Be201Created();
        response.Should().BeAs(new
        {
            Name = "Kyle",
            Email = "alpaca@llama.com",
            Date = "25th Apr Monday",
            Type = "Consultation"
        });
    }

    [Test]
    public async Task PostAppointment_respond_with_400_for_incomplete_data()
    {
        // Arrange
        var testJsonObj = new
        {
            Name = "Alpaca",
            Email = "llama@kyle.com",
            Type = "Consultation"
            // date field missing
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/appointment", testJsonObj);
        var contentJson = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().Be400BadRequest();
        response.Should().HaveErrorMessage("The Date field is required.");
    }

    [Test]
    public async Task PostAppointment_respond_with_400_for_req_with_no_body()
    {
        var response = await _httpClient.PostAsJsonAsync("/api/appointment", new { });
        var contentJson = await response.Content.ReadAsStringAsync();

        response.Should().Be400BadRequest();
        response.Should().HaveErrorMessage("The Name field is required.");
        response.Should().HaveErrorMessage("The Email field is required.");
        response.Should().HaveErrorMessage("The Type field is required.");
        response.Should().HaveErrorMessage("The Date field is required.");
    }

    [Test]
    public async Task PostAppointment_successful_run_add_new_record_in_database()
    {
        // Arrange
        var testJsonObj = new
        {
            Name = "Kyle",
            Email = "kyle@llama.com",
            Date = "25th Mon 14:00",
            Type = "Consultation"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/appointment", testJsonObj);
        var contentJson = await response.Content.ReadAsStringAsync();

        // Assert
        response.Should().Be201Created();
        var responseBody = JObject.Parse(contentJson);
        int? newRecordId = responseBody["id"]?.Value<int>();
        var newRecord = _dbContext.Appointments.Find(newRecordId);

        newRecord.Should().NotBeNull();
        if (newRecord is not null)
        {
            newRecord.Name.Should().Be("Kyle");
            newRecord.Email.Should().Be("kyle@llama.com");
            newRecord.Type.Should().Be("Consultation");
            newRecord.Date.Should().Be("25th Mon 14:00");
        }
    }
}