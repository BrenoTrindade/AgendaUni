using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendaUni.Repositories
{
    public class AbsenceRepository : IAbsenceRepository
    {
        private readonly AppDbContext _context;

        public AbsenceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Absence absence)
        {
            await _context.Absences.AddAsync(absence);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Absence>> GetAllAsync()
        {
            return await _context.Absences.ToListAsync();
        }

        public async Task<Absence> GetByIdAsync(int id)
        {
            return await _context.Absences.FindAsync(id);
        }

        public async Task UpdateAsync(Absence absence)
        {
            _context.Absences.Update(absence);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var absence = await _context.Absences.FindAsync(id);
            if (absence != null)
            {
                _context.Absences.Remove(absence);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Absence>> GetAbsencesByClassIdAsync(int classId)
        {
            return await _context.Absences
                .Where(a => a.ClassId == classId)
                .ToListAsync();
        }
    }
}
