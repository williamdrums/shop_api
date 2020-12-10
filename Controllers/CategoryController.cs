using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;

namespace Shop.Controllers
{
    [Route("categories")]
    public class Controller : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<Category>> Get()
        {
            return new Category();
        }

        [HttpGet]
        [Route("{id:int}")]
        public string GetId()
        {
            return "Get";
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Category>> Post(
            [FromBody] Category model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception e)
            {
               return BadRequest(new{message = "Não foi possivel criar a categoria",e});
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model)
        {
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada!" });
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(model);
        }

        //   [HttpDelete]
        // [Route("{int:id}")]
        // public Category Delete(int id)
        // {
        //     return id;
        // }
    }
}