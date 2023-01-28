using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        [HttpGet("/")]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
