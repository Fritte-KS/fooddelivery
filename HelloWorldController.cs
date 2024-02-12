using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("helloworld")]
public class HelloWorldController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHellorWorldString()
    {
        return Ok("Hello world");
    }
}

// webApp.MapGet("/helloworld", () => "hellow world");