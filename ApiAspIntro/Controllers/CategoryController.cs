
using ApiAspIntro.DAL;
using ApiAspIntro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAspIntro.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Category category)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var datas = await _context.Categories.FindAsync(id);
            if (datas == null) return NotFound();
            return Ok(datas);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, Category create)
        {
            if (id == null) return BadRequest();
            Category datas = await _context.Categories.FindAsync(id);
            if (!ModelState.IsValid)
            {
                create.Name = datas.Name;
                return Ok(create);

            }
            if (datas == null) return NotFound();

            datas.Name = create.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }



        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (id == null) return BadRequest();
            var datas = await _context.Categories.FindAsync(id);
            if (datas == null) return NotFound();

            _context.Categories.Remove(datas);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
