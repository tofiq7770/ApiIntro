using ApiAspIntro.DAL;
using ApiAspIntro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAspIntro.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CityController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] City city, int countryId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool datas = await _context.Cities.AnyAsync(m => m.Name.Trim() == city.Name.Trim());
            if (datas)
            {
                ModelState.AddModelError("Name", "City already exists");
                return BadRequest(ModelState);
            }


            var datasOfCity = await _context.Countries.FindAsync(countryId);
            if (datasOfCity == null && city.Id != countryId)
            {
                ModelState.AddModelError("CountryId", "No Country was Found!");
                return BadRequest(ModelState);
            }

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), city);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Cities.ToListAsync());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, City newData)
        {
            if (id == null)
                return BadRequest();

            var datasOfCity = await _context.Cities.FindAsync(id);
            if (datasOfCity == null)
                return NotFound();

            datasOfCity.Name = newData.Name;

            if (newData.CountryId != 0)
            {
                var datas = await _context.Countries.FindAsync(newData.CountryId);
                if (datas == null)
                {
                    ModelState.AddModelError("CountryId", "Invalid city ID");
                    return BadRequest(ModelState);
                }


                datasOfCity.CountryId = newData.CountryId;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (id == null) return BadRequest();
            var datas = await _context.Cities.FindAsync(id);
            if (datas == null) return NotFound();

            _context.Cities.Remove(datas);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
