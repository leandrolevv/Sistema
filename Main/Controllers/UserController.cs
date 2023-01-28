﻿using System.Data;
using System.Data.Common;
using Main.DbContextSistema;
using Main.Extension;
using Main.Models;
using Main.ViewModel;
using Main.ViewModel.EditorViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureIdentity.Password;

namespace Main.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("v1/users")]
        public async Task<ActionResult> CreateAsync([FromServices] DbContextAccount context,
            [FromBody] EditorUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel<string>(ModelState.GetErrors()));
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

        [HttpGet("/v1/users")]
        public async Task<ActionResult> GetAsync([FromServices] DbContextAccount context,
            [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 25)
        {
            try
            {
                var users = await context
                    .Users
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new ResponseViewModel<List<User>>(users));
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

        [HttpGet("/v1/users/{id}")]
        public async Task<ActionResult> GetByIdAsync([FromServices] DbContextAccount context, [FromRoute] int id)
        {
            try
            {
                var user = await context
                    .Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                return Ok(new ResponseViewModel<User>(user));
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
