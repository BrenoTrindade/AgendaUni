using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendaUni.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class> GetByIdAsync(int id)
        {
            return await _context.Classes.FindAsync(id);
        }

        public async Task<Class> AddAsync(Class classObj)
        {
            _context.Classes.Add(classObj);
            await _context.SaveChangesAsync();
            return classObj;
        }

        public async Task UpdateAsync(Class classObj)
        {
            _context.Classes.Update(classObj);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var classToDelete = await _context.Classes.FindAsync(id);

            if (classToDelete != null)
            {
                _context.Classes.Remove(classToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
