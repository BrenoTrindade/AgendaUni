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

        public async Task<ServiceResult> AddEventAsync(Event ev)
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

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _eventRepository.GetByIdAsync(id);
        }


        public async Task<ServiceResult> UpdateEventAsync(Event ev)
        {
            if (ev.ClassId == 0)
                return ServiceResult.Failure("Selecione uma aula.");

            if (string.IsNullOrWhiteSpace(ev.Description))
                return ServiceResult.Failure("Informe a descrição do evento.");

            await _eventRepository.UpdateAsync(ev);

            return ServiceResult.Success("Evento atualizado com sucesso.");
        }

        public async Task<ServiceResult> DeleteEventAsync(int id)
        {
            var eventToDelete = await _eventRepository.GetByIdAsync(id);
            if (eventToDelete == null)
                return ServiceResult.Failure("Evento não encontrado.");

            await _eventRepository.DeleteAsync(id);

            return ServiceResult.Success("Evento deletado com sucesso.");
        }
    }
}