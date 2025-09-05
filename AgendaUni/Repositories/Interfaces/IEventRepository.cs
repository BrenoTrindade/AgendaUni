using AgendaUni.Models;

namespace AgendaUni.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task AddAsync(Event ev);
        Task<IEnumerable<Event>> GetAllAsync();
    }
}