using AgendaUni.Models;

namespace AgendaUni.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task AddAsync(Event ev);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(int id);
    }
}