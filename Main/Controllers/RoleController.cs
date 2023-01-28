using Main.DbContextSistema;
using Main.Models;
using Main.ViewModel;
using Main.ViewModel.EditorViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using Main.Extension;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Main.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        [Authorize(Roles = "administrador")]
        [HttpPost("v1/roles")]
        public async Task<ActionResult> CreateAsync([FromServices] DbContextAccount context, [FromBody] EditorRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel<string>(ModelState.GetErrors()));
            }

            try
            {
                var role = new Role()
                {
                    Name = model.Name,
                    Slug = model.Name.ToLower().Replace(" ", "_")
                };

                await context.AddAsync(role);
                await context.SaveChangesAsync();

                return Created($"v1/roles/{role.Slug}", new ResponseViewModel<Role>(role));
            }
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-01 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-01 - Ocorreu um erro no servidor") );
            }
        }
    }
}
