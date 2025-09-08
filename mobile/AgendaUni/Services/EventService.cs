using AgendaUni.Common;
using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;

namespace AgendaUni.Services
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<ServiceResult> RegisterEventAsync(Event ev)
        {
            if (ev.ClassId == 0)
                return ServiceResult.Failure("Selecione uma aula.");

            if (string.IsNullOrWhiteSpace(ev.Description))
                return ServiceResult.Failure("Informe a descrição do evento.");

            await _eventRepository.AddAsync(ev);

            return ServiceResult.Success("Evento registrado com sucesso.");
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.GetAllAsync();
        }
    }
}