namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Components.Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("root")]
    public IActionResult Get2(
        [FromServices] IConfiguration config)
    {
        var env = config.GetValue<string>("Env");
        return Ok(new
        {
            environment = env
        });
    }

    
    [HttpGet("")]

    public Response<string> Get(
        [FromServices] IConfiguration config)
    {
        var env = config.GetValue<string>("Env");
        var environament = new Response<string>(env);
        return new Response<string>(null, 200, $"Environament: {env}");
        
    }
    
    [HttpGet("health")]
    public Response<string> Health()
    {
        return new Response<string>("API => Ok");
    }

    [HttpGet("version")]
    public Response<string> Version()
    {
        return new Response<string>("1.0.0");
    }

    [HttpGet("/home")]
    public void GetHome(
        [FromBody] UserStateService userState,
        [FromServices] NavigationManager navigationManager)
    {
        var userId = userState.GetLoggedUserId();
        if (userId == 0)
        {
            navigationManager.NavigateTo("/logout");
        }
        //return Ok();
    }
}