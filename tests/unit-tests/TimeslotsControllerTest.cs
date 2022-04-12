using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using project.Data;

namespace unit_tests;

public class TimeslotsControllerTest
{
    private readonly HttpClient _httpClient;

    public TimeslotsControllerTest()
    {
        _httpClient = new WebApplicationFactory<project.Startup>().CreateClient();

    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetTimeslots_responds_with_OK()
    {
        var response = await _httpClient.GetAsync("/api/timeslots");

        response.Should().Be200Ok();
    }
    
    [Test]
    public async Task GetTimeslots_responds_with_an_array_of_timeslots()
    {
        var response = await _httpClient.GetAsync("/api/timeslots");
        var contentJson = await response.Content.ReadAsStringAsync();
        var responseBody = Helper.ParseJsonToObject<string[]>(contentJson);

        response.Should().Be200Ok();
        responseBody.Should().NotBeNullOrEmpty();
        
        // should return an array of datetime in string,
        // where every datetime should be after today & within working hour
        responseBody.Should().AllSatisfy(str =>
        {
            var date = DateTime.Parse(str);
            date.Should().BeAfter(DateTime.Today);
            date.DayOfWeek.Should().NotBe(DayOfWeek.Saturday).And.NotBe(DayOfWeek.Sunday);
            date.Hour.Should().BeInRange(8, 18);
        });

    }
}