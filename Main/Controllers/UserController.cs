﻿using System.Data;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Azure.Storage.Blobs;
using Main.DbContextSistema;
using Main.Extension;
using Main.Models;
using Main.ViewModel;
using Main.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using SecureIdentity.Password;

namespace Main.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [Authorize(Roles = "administrador")]
        [HttpPost("/v1/users")]
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
                    Slug = model.Email.CreateSlug(),
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

        [Authorize(Roles = "administrador")]
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
                return BadRequest(new ResponseViewModel<string>("DB-02 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-02 - Ocorreu um erro no servidor"));
            }
        }

        [Authorize(Roles = "administrador")]
        [HttpGet("/v1/users/{id}")]
        public async Task<ActionResult> GetByIdAsync([FromServices] DbContextAccount context, [FromRoute] int id)
        {
            try
            {
                var user = await context
                    .Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new ResponseViewModel<string>("Não foi possível encontrar o usuário"));
                }

                return Ok(new ResponseViewModel<User>(user));
            }
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-03 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-03 - Ocorreu um erro no servidor"));
            }
        }

        [Authorize(Roles = "administrador")]
        [HttpDelete("/v1/users/{id}")]
        public async Task<ActionResult> DeleteAsync([FromServices] DbContextAccount context, [FromRoute] int id)
        {
            try
            {
                var user = await context
                    .Users
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new ResponseViewModel<string>("Não foi possível encontrar o usuário"));
                }

                context.Remove(user);
                await context.SaveChangesAsync();

                return Ok(new ResponseViewModel<dynamic>(new { sucesso = "Deletado com sucesso" }));
            }
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-04 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-04 - Ocorreu um erro no servidor"));
            }
        }

        [HttpPut("/v1/users/ChangePic")]
        public async Task<ActionResult> PostAsync([FromServices] DbContextAccount context, [FromBody] ChangePicUserViewModel model)
        {
            try
            {
                var user = await context
                    .Users
                    .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

                if (user == null)
                {
                    return BadRequest(new ResponseViewModel<string>("Não foi possível encontrar o usuário"));
                }

                var imageName = User.Identity.Name + "_ProfilePic";
                var imageBase64 = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");
                var file = Convert.FromBase64String(imageBase64);
                var blobClient = new BlobClient(Configuration.AzureBlobConnectionString, "containeruserimages",
                    imageName);


                using(var stream = new MemoryStream(file))
                {
                    await blobClient.UploadAsync(stream);
                }

                user.linkProfileImage = blobClient.Uri.AbsoluteUri;

               context.Update(user);
               await context.SaveChangesAsync(); 

                return Ok(user);
            }
            catch (DbException)
            {
                return BadRequest(new ResponseViewModel<string>("DB-08 - Ocorreu um erro no banco de dados"));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseViewModel<string>("EG-08 - Ocorreu um erro no servidor"));
            }
        }
    }
}
