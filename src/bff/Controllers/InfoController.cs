using API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Controllers;

public class InfoController : BaseApiController
{
    [HttpGet("bff-info")]
    public ActionResult GetNotFound()
    {
        var info = new
        {
            Bff = "BFF for the project",
            Version = "1.0.0",
        };
        return Ok(info);
    }
}