
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("v1/users")]
    public class UserController : Controller
    {

        [HttpGet]
        [Route("find")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> GetUsers([FromServices] DataContext context)
        {
            var users = await context
                .Users
                .AsNoTracking()
                .ToListAsync();
            return users;
        }


        [HttpPost]
        [Route("save")]
        [AllowAnonymous]
        // [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Post(
           [FromServices] DataContext context,
           [FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //Força o usuario a sempre sem funcionario
                model.Role = "employee";

                context.Users.Add(model);
                await context.SaveChangesAsync();

                //Esconde a senha
                model.Password ="";
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar usuario" });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
                    [FromServices] DataContext context,
                    [FromBody] User model)
        {
            var user = await context.Users
            .AsNoTracking()
            .Where(x => x.Username == model.Username && x.Password == model.Password)
            .FirstOrDefaultAsync();


            if (user == null)
                return NotFound(new { message = "Usuario ou Senha invalido" });

            var token = TokenService.GenerateToken(user);

            //Esconde a senha
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPut]
        [Route("update/{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put([FromServices] DataContext context,
        int id, [FromBody] User model)
        {
            //verifica se os dados são validos
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id != model.Id)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar usuário" });
           }

        }
        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<ActionResult<User>> DeleteUser(int id, [FromServices] DataContext context)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok(new { message = "Usuário removido com sucesso!" });
            }
            catch (Exception)
            {
                return NotFound(new { message = "Não foi possivel remover o usuario" });
            }
        }
    }
}