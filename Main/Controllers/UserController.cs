using Main.DbContextSistema;
using Main.Models;
using Main.ViewModel.EditorViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var roles = await context.Roles.Where(r => model.Roles.Contains(r.Slug)).ToListAsync();

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

            return Created($"/{user.Slug}", user);
        }
    }
}
