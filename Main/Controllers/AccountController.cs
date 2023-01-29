using Main.DbContextSistema;
using Main.Extension;
using Main.Models;
using Main.Services;
using Main.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Data.Common;

namespace Main.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("/v1/Login")]
        public async Task<ActionResult> Get([FromServices] DbContextAccount context, [FromServices] TokenService tokenService, [FromBody] LoginViewModel model)
        {
            try
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
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-06 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-06 - Ocorreu um erro no servidor"));
            }
        }

        [HttpPost("/v1/CreateAccount")]
        public async Task<ActionResult> CreateAsync([FromServices] DbContextAccount context,
            [FromBody] AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel<string>(ModelState.GetErrors()));
            }

            try
            {
                var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == "usuario");

                if (role == null)
                {
                    return BadRequest(new ResponseViewModel<string>("O grupo informado é inválido"));
                }

                var user = new User()
                {
                    Email = model.Email,
                    Name = model.Name,
                    PasswordHash = PasswordHasher.Hash(model.Password),
                    Slug = model.Email.CreateSlug(),
                    Roles = new List<Role>() {role}
            };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Created($"/{user.Slug}", new ResponseViewModel<User>(user));
            }
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-07 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-07 - Ocorreu um erro no servidor"));
            }
        }
    }
}
