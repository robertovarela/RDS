namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Components.Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get(
        [FromServices] IConfiguration config)
    {
        var env = config.GetValue<string>("Env");
        return Ok(new
        {
            environment = env
        });
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok("ok");
    }

    [HttpGet("version")]
    public IActionResult Version()
    {
        return Ok("1.0.0");
    }

    [HttpGet("/home")]
    public IActionResult GetHome(
        [FromBody] UserStateService userState,
        [FromServices] NavigationManager navigationManager)
    {
        var userId = userState.GetLoggedUserId();
        if (userId == 0)
        {
            navigationManager.NavigateTo("/logout");
        }
        return Ok();
    }
}