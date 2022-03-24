using Microsoft.AspNetCore.Mvc;

namespace project.Controllers;


[ApiController]
[Route("")]
public class BaseController : ControllerBase
{
    [HttpGet]
     public string Get()
     {
         return "Hello World! There's nothing here. Go and see the endpoint: /api/notes !";
     }
}