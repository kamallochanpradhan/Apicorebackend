using AngularCrudApI1.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularCrudApI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyController : ControllerBase
    {
        private readonly MyAngularDataContext _appDBContext;

        public AgencyController(MyAngularDataContext context)
        {
            _appDBContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agency>>> GetAll()
        {
            var agencies = await _appDBContext.Agencies.ToListAsync();
            return Ok(agencies); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Agency>> GetById(int id)
        {
            var agency = await _appDBContext.Agencies.FindAsync(id);
            if (agency == null) return NotFound();
            return agency;
        }

        [HttpPost]
        public async Task<ActionResult<Agency>> Create(Agency agency)
        {
            _appDBContext.Agencies.Add(agency);
            await _appDBContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = agency.Id }, agency);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Agency agency)
        {
            if (id != agency.Id) return BadRequest();

            _appDBContext.Entry(agency).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var agency = await _appDBContext.Agencies.FindAsync(id);
            if (agency == null) return NotFound();

            _appDBContext.Agencies.Remove(agency);
            await _appDBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
