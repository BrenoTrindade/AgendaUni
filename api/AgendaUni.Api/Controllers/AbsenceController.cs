using Microsoft.AspNetCore.Authorization;
using AgendaUni.Api.Models;
using Microsoft.AspNetCore.Mvc;
using AgendaUni.Api.Services.Interfaces;

namespace AgendaUni.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User, Admin")]
    public class AbsenceController : ControllerBase
    {
        private readonly IAbsenceService _absenceService;

        public AbsenceController(IAbsenceService absenceService)
        {
            _absenceService = absenceService;
        }

        // GET: api/Absence
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Absence>>> GetAbsences()
        {
            return Ok(await _absenceService.GetAllAbsences());
        }

        // GET: api/Absence/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Absence>> GetAbsence(int id)
        {
            var absence = await _absenceService.GetAbsenceById(id);

            if (absence == null)
            {
                return NotFound();
            }

            return Ok(absence);
        }

        // PUT: api/Absence/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbsence(int id, Absence absence)
        {
            if (id != absence.Id)
            {
                return BadRequest();
            }

            await _absenceService.UpdateAbsence(absence);

            return NoContent();
        }

        // POST: api/Absence
        [HttpPost]
        public async Task<ActionResult<Absence>> PostAbsence(Absence absence)
        {
            await _absenceService.AddAbsence(absence);

            return CreatedAtAction("GetAbsence", new { id = absence.Id }, absence);
        }

        // DELETE: api/Absence/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbsence(int id)
        {
            var absence = await _absenceService.GetAbsenceById(id);
            if (absence == null)
            {
                return NotFound();
            }

            await _absenceService.DeleteAbsence(id);

            return NoContent();
        }
    }
}