using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendaUni.Repositories
{
    public class ClassScheduleRepository : IClassScheduleRepository
    {
        private readonly AppDbContext _context;

        public ClassScheduleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ClassSchedule classSchedule)
        {
            await _context.ClassSchedules.AddAsync(classSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClassSchedule>> GetAllAsync()
        {
            return await _context.ClassSchedules.ToListAsync();
        }

        public async Task<ClassSchedule> GetByIdAsync(int id)
        {
            return await _context.ClassSchedules.FindAsync(id);
        }

        public async Task UpdateAsync(ClassSchedule classSchedule)
        {
            _context.ClassSchedules.Update(classSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var classSchedule = await _context.ClassSchedules.FindAsync(id);
            if (classSchedule != null)
            {
                _context.ClassSchedules.Remove(classSchedule);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ClassSchedule>> GetSchedulesByClassIdAsync(int classId)
        {
            return await _context.ClassSchedules
                .Where(cs => cs.ClassId == classId)
                .ToListAsync();
        }
    }
}
