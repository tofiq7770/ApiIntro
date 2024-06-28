using ApiAspIntro.DAL;
using ApiAspIntro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAspIntro.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CountryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Country country)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            bool datas = await _context.Countries.AnyAsync(m => m.Name.Trim() == country.Name.Trim());
            if (datas)
            {
                ModelState.AddModelError("Name", "Country already exist");
                return Ok();
            }
            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), country);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Countries.Include(m => m.Cities).ToListAsync());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, Country create)
        {
            if (id == null) return BadRequest();
            var datas = await _context.Countries.FindAsync(id);
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
            var datas = await _context.Countries.FindAsync(id);
            if (datas == null) return NotFound();
            _context.Countries.Remove(datas);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
