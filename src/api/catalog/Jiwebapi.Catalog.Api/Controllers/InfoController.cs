using Microsoft.AspNetCore.Mvc;

namespace Jiwebapi.Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Get()
        {
            return Ok(new
            {
                env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                server = Environment.MachineName,
                //now = DateTime.UtcNow,
            });
        }
    }
}
