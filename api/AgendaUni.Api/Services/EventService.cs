using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using AgendaUni.Api.Services.Interfaces;

namespace AgendaUni.Api.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task AddEvent(Event @event)
        {
            await _eventRepository.Add(@event);
            await _eventRepository.Save();
        }

        public async Task DeleteEvent(int id)
        {
            var @event = await _eventRepository.GetById(id);
            if (@event != null)
            {
                _eventRepository.Delete(@event);
                await _eventRepository.Save();
            }
        }

        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await _eventRepository.GetAll();
        }

        public async Task<Event> GetEventById(int id)
        {
            return await _eventRepository.GetById(id);
        }

        public async Task UpdateEvent(Event @event)
        {
            _eventRepository.Update(@event);
        }
    }
}