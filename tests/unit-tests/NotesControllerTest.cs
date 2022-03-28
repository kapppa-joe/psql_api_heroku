using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

using NUnit.Framework;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using project.Data;

namespace unit_tests;
public class TestNotesController
{
    private HttpClient _httpClient;
    private readonly WebApplicationFactory<project.Startup> _factory;
    

    public TestNotesController()
    {
        
        _factory = new WebApplicationFactory<project.Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // remove the dbcontext of actual postgres db and stub it with a in memory sqlite db
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof (DbContextOptions < PsqlDbContext > ));

                    if (descriptor != null) {
                        services.Remove(descriptor);
                    }
                    
                    services.AddDbContext<PsqlDbContext>(opt =>
                        opt.UseInMemoryDatabase("test_database"));
                });
            }
        );
        _httpClient = _factory.CreateClient();
    }

    [SetUp]
    public void Setup()
    {
        _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Test]
    public async Task GetNotes_responds_with_OK()
    {
        var response = await _httpClient.GetAsync("/api/Notes");

        response.Should().Be200Ok();
    }

    // [Test]
    // public void PostNotes_responds_with_200()
    // {
    //     var options = new DbContextOptionsBuilder<PsqlDbContext>()
    //         .UseInMemoryDatabase(databaseName: "Test")
    //         .Options;
    //     var mockContext = new PsqlDbContext(options);
    //
    //     var controller = new NotesController(mockContext);
    //     var newNote = new Note();
    //     newNote.Title = "test title";
    //     newNote.Content = "test content";
    //     
    //     var response = controller.PostNote(newNote);
    //
    //     mockContext.Notes.Count().Should().Be(1);a
    //
    //     // mockSet.Verify(m => m.Add(It.IsAny<Note>()), Times.Once());
    //     // mockContext.Verify(m => m.SaveChanges(), Times.Once());
    // }
}