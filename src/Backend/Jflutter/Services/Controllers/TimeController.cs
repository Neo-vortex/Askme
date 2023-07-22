using Microsoft.AspNetCore.Mvc;

namespace Jflutter.Services.Controllers;

public class TimeController : Controller
{
    private readonly ILogger<TimeController> _logger;

    public TimeController(ILogger<TimeController> logger)
    {
        _logger = logger;
    }
    // GET
    [HttpGet]
    [Route("[controller]")]
    public  ActionResult<long>  Index()
    {
        _logger.LogInformation("Time endpoint hit!");
        return Ok( DateTimeOffset.Now.ToUnixTimeMilliseconds());
    }
}