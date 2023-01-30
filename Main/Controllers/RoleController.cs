using Main.DbContextSistema;
using Main.Models;
using Main.ViewModel;
using Main.ViewModel.EditorViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using Main.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Main.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        [Authorize(Roles = Constantes.RoleConstante.ADMIN)]
        [HttpPost("v1/roles")]
        public async Task<ActionResult> CreateAsync([FromServices] DbContextAccount context,
            [FromBody] EditorRoleViewModel model)
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
                return BadRequest(new ResponseViewModel<string>("EG-05 - Ocorreu um erro no servidor"));
            }
        }

        [HttpGet("v1/roles")]
        public async Task<ActionResult> GetAsync([FromServices] DbContextAccount context)
        {
            try{
                var roles = await context.Roles.AsNoTracking().ToListAsync();

                return Ok(new ResponseViewModel<IList<Role>>(roles));
            }
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-09 - Ocorreu um erro no banco de dados"));
            }

            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-09 - Ocorreu um erro no servidor"));
            }
        }

        [Authorize(Roles = Constantes.RoleConstante.ADMIN)]
        [HttpDelete("/v1/roles/{id}")]
        public async Task<ActionResult> DeleteAsync([FromServices] DbContextAccount context, [FromRoute] int id)
        {
            try
            {
                var role = await context
                    .Roles
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (role == null)
                {
                    return NotFound(new ResponseViewModel<string>("O grupo não existe"));
                }

                if (Configuration.ProtectedRoles.Contains(role.Name.CreateSlug()))
                {
                    return BadRequest(new ResponseViewModel<string>($"O grupo {role.Name} não pode ser excluído"));
                }

                context.Remove(role);
                await context.SaveChangesAsync();

                return Ok(new ResponseViewModel<dynamic>(new { sucesso = "Deletado com sucesso" }));
            }
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-10 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-10 - Ocorreu um erro no servidor"));
            }
        }

    }
}


