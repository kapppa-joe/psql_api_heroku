using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeSlotsController : ControllerBase
{
    // GET: api/timeslots
    [HttpGet]
    public ActionResult GetTimeslots()
    {
        var today = DateTime.Today;
        var next28Days = Enumerable.Range(1, 28).Select(x =>
        {
            // var days_added = new TimeSpan(24 * x, 0, 0);
            var newDate = DateTime.Today.AddDays(x);
            var date = new DateTime(newDate.Year, newDate.Month, newDate.Day, 11, 0, 0);
            return date;
        });
        var workDaysOnly = next28Days.Where(date =>
            date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);

        string[] timeslots = workDaysOnly.Select(date => date.ToString("yyyy-MM-ddThh:mm")).ToArray();
        return Ok(timeslots);
    }
}