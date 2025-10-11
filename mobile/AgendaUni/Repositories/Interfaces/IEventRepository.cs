using AgendaUni.Models;

namespace AgendaUni.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task AddAsync(Event ev, IEnumerable<int> notificationIds);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task UpdateAsync(Event ev, IEnumerable<int> notificationIds);
        Task DeleteAsync(int id);
        Task<IEnumerable<Event>> GetEventsByClassIdAsync(int classId);
    }
}