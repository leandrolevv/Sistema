using Main.DbContextSistema;
using Main.Models;
using Main.ViewModel;
using Main.ViewModel.EditorViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using Main.Extension;
using Microsoft.AspNetCore.Authorization;

namespace Main.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        [Authorize(Roles = Constantes.RoleConstante.ADMIN)]
        [HttpPost("v1/roles")]
        public async Task<ActionResult> CreateAsync([FromServices] DbContextAccount context, [FromBody] EditorRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel<string>(ModelState.GetErrors()));
            }

            ;

            if (Configuration.ProtectedRoles.Contains(model.Name.CreateSlug()))
            {
                return BadRequest(new ResponseViewModel<string>($"O grupo {model.Name} não pode ser excluído"));
            }

            try
            {
                var role = new Role()
                {
                    Name = model.Name,
                    Slug = model.Name.CreateSlug()
                };

                await context.AddAsync(role);
                await context.SaveChangesAsync();

                return Created($"v1/roles/{role.Slug}", new ResponseViewModel<Role>(role));
            }
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-05 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-05 - Ocorreu um erro no servidor") );
            }
        }
    }
}
