using Microsoft.AspNetCore.Authorization;
using AgendaUni.Api.Models;
using Microsoft.AspNetCore.Mvc;
using AgendaUni.Api.Services.Interfaces;

namespace AgendaUni.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User, Admin")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        // GET: api/Class
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            return Ok(await _classService.GetAllClasses());
        }

        // GET: api/Class/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var @class = await _classService.GetClassById(id);

            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }

        // PUT: api/Class/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(int id, Class @class)
        {
            if (id != @class.Id)
            {
                return BadRequest();
            }

            await _classService.UpdateClass(@class);

            return NoContent();
        }

        // POST: api/Class
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(Class @class)
        {
            await _classService.AddClass(@class);

            return CreatedAtAction("GetClass", new { id = @class.Id }, @class);
        }

        // DELETE: api/Class/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var @class = await _classService.GetClassById(id);
            if (@class == null)
            {
                return NotFound();
            }

            await _classService.DeleteClass(id);

            return NoContent();
        }
    }
}