using Main.DbContextSistema;
using Main.Extension;
using Main.Models;
using Main.Services;
using Main.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Main.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [Authorize(Roles = "administrador")]
        [HttpGet("/v1/Login")]
        public async Task<ActionResult> Get([FromServices] DbContextAccount context, [FromServices] TokenService tokenService, [FromBody] LoginViewModel model)
        {
            var user = await context.Users.Include(x => x.Roles).AsNoTracking().Where(x => x.Email == model.Email).FirstOrDefaultAsync();

            if (model == null)
            {
                return BadRequest("Usuário ou senha inválidos");
            }

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            {
               return BadRequest("Usuário ou senha inválidos");
            }
            
            var token = tokenService.GetToken(user);

            return Ok(token);
        }
    }
}
