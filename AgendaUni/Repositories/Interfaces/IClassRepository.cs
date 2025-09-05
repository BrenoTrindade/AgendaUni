using AgendaUni.Models;

namespace AgendaUni.Repositories.Interfaces
{
    public interface IClassRepository
    {
        Task<List<Class>> GetAllAsync();
        Task<Class> GetByIdAsync(int id);
        Task AddAsync(Class classObj);
    }
}
