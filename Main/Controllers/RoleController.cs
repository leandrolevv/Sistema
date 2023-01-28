using Main.DbContextSistema;
using Main.Models;
using Main.ViewModel.EditorViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        [HttpPost("v1/roles")]
        public async Task<ActionResult> CreateAsync([FromServices] DbContextAccount context, [FromBody] EditorRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var role = new Role()
            {
                Name = model.Name,
                Slug = model.Name.ToLower().Replace(" ", "_")
            };

            await context.AddAsync(role);
            await context.SaveChangesAsync();

            return Created($"v1/roles/{role.Slug}", role);
        }
    }
}
