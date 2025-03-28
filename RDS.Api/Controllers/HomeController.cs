namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("root")]
    public IActionResult Get2([FromServices] IConfiguration config)
    {
        var env = config.GetValue<string>("Env");
        return Ok(new
        {
            environment = env
        });
    }
    
    [HttpGet("")]
    public Response<string> Get([FromServices] IConfiguration config)
    {
        var env = config.GetValue<string>("Env");

        return new Response<string>($"Environament => {env}");
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
}