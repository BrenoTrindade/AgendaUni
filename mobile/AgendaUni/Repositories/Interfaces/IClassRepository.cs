using AgendaUni.Models;

namespace AgendaUni.Repositories.Interfaces
{
    public interface IClassRepository
    {
        Task<List<Class>> GetAllAsync();
        Task<Class> GetByIdAsync(int id);
        Task<Class> AddAsync(Class classObj);
        Task UpdateAsync(Class classObj);
        Task DeleteAsync(int id);
    }
}
