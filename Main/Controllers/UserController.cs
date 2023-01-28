using System.Data;
using System.Data.Common;
using Main.DbContextSistema;
using Main.Models;
using Main.ViewModel;
using Main.ViewModel.EditorViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureIdentity.Password;

namespace Main.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("v1/users")]
        public async Task<ActionResult> CreateAsync([FromServices] DbContextAccount context, [FromBody] EditorUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            try
            {
                var roles = await context.Roles.Where(r => model.Roles.Contains(r.Slug)).ToListAsync();

                if (roles.IsNullOrEmpty())
                {
                    return BadRequest(new ResponseViewModel<string>("O grupo informado é inválido"));
                }

                var user = new User()
                {
                    Email = model.Email,
                    Name = model.Name,
                    PasswordHash = PasswordHasher.Hash(model.Password),
                    Slug = model.Email.ToLower().Replace("@", "-").Replace(".", "-"),
                    Roles = roles
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Created($"/{user.Slug}", new ResponseViewModel<User>(user));
            }
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-01 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-01 - Ocorreu um erro no servidor"));
            }
        }
    }
}
