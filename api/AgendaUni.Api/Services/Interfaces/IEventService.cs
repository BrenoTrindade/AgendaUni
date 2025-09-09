using AgendaUni.Api.Models;

namespace AgendaUni.Api.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEvents();
        Task<Event> GetEventById(int id);
        Task AddEvent(Event @event);
        Task UpdateEvent(Event @event);
        Task DeleteEvent(int id);
    }
}