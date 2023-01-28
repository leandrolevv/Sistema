using Main.Models;
using Main.Services;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("/v1/Login")]
        public async Task<ActionResult> Get([FromServices] TokenService tokenService)
        {
            var token = tokenService.GetToken(new User());

            return Ok(token);
        }
    }
}
