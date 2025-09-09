using AgendaUni.Api.Models;
using Microsoft.AspNetCore.Mvc;
using AgendaUni.Api.Services.Interfaces;

namespace AgendaUni.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassScheduleController : ControllerBase
    {
        private readonly IClassScheduleService _classScheduleService;

        public ClassScheduleController(IClassScheduleService classScheduleService)
        {
            _classScheduleService = classScheduleService;
        }

        // GET: api/ClassSchedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassSchedule>>> GetClassSchedules()
        {
            return Ok(await _classScheduleService.GetAllClassSchedules());
        }

        // GET: api/ClassSchedule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassSchedule>> GetClassSchedule(int id)
        {
            var classSchedule = await _classScheduleService.GetClassScheduleById(id);

            if (classSchedule == null)
            {
                return NotFound();
            }

            return Ok(classSchedule);
        }

        // PUT: api/ClassSchedule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassSchedule(int id, ClassSchedule classSchedule)
        {
            if (id != classSchedule.Id)
            {
                return BadRequest();
            }

            await _classScheduleService.UpdateClassSchedule(classSchedule);

            return NoContent();
        }

        // POST: api/ClassSchedule
        [HttpPost]
        public async Task<ActionResult<ClassSchedule>> PostClassSchedule(ClassSchedule classSchedule)
        {
            await _classScheduleService.AddClassSchedule(classSchedule);

            return CreatedAtAction("GetClassSchedule", new { id = classSchedule.Id }, classSchedule);
        }

        // DELETE: api/ClassSchedule/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassSchedule(int id)
        {
            var classSchedule = await _classScheduleService.GetClassScheduleById(id);
            if (classSchedule == null)
            {
                return NotFound();
            }

            await _classScheduleService.DeleteClassSchedule(id);

            return NoContent();
        }
    }
}