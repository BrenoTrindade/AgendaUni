using AgendaUni.Common;
using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;

namespace AgendaUni.Services
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IClassRepository _classRepository;
        private readonly NotificationService _notificationService;

        public EventService(IEventRepository eventRepository, IClassRepository classRepository, NotificationService notificationService)
        {
            _eventRepository = eventRepository;
            _classRepository = classRepository;
            _notificationService = notificationService;
        }

        public async Task<ServiceResult> AddEventAsync(Event ev)
        {
            if (ev.ClassId == 0)
                return ServiceResult.Failure("Selecione uma aula.");

            if (string.IsNullOrWhiteSpace(ev.Description))
                return ServiceResult.Failure("Informe a descrição do evento.");

            var cl = await _classRepository.GetByIdAsync(ev.ClassId);

            var notificationIds = await _notificationService.ScheduleNotificationForEvent(ev, cl);

            await _eventRepository.AddAsync(ev, notificationIds);

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

        public async Task<IEnumerable<Event>> GetEventsByClassIdAsync(int classId)
        {
            return await _eventRepository.GetEventsByClassIdAsync(classId);
        }


        public async Task<ServiceResult> UpdateEventAsync(Event ev)
        {
            if (ev.ClassId == 0)
                return ServiceResult.Failure("Selecione uma aula.");

            if (string.IsNullOrWhiteSpace(ev.Description))
                return ServiceResult.Failure("Informe a descrição do evento.");

            var existingEvent = await _eventRepository.GetByIdAsync(ev.Id);
            if (existingEvent?.EventNotifications != null)
            {
                foreach (var notification in existingEvent.EventNotifications)
                {
                    _notificationService.CancelNotification(notification.NotificationId);
                }
            }

            var cl = await _classRepository.GetByIdAsync(ev.ClassId);

            if (cl == null)
            {
                return ServiceResult.Failure("A aula associada a este horário não foi encontrada.");
            }
            var notificationIds = await _notificationService.ScheduleNotificationForEvent(ev, cl);

            await _eventRepository.UpdateAsync(ev, notificationIds);

            return ServiceResult.Success("Evento atualizado com sucesso.");
        }

        public async Task<ServiceResult> DeleteEventAsync(int id)
        {
            var eventToDelete = await _eventRepository.GetByIdAsync(id);
            if (eventToDelete == null)
                return ServiceResult.Failure("Evento não encontrado.");

            if (eventToDelete.EventNotifications != null)
            {
                foreach (var notification in eventToDelete.EventNotifications)
                {
                    _notificationService.CancelNotification(notification.NotificationId);
                }
            }

            await _eventRepository.DeleteAsync(id);

            return ServiceResult.Success("Evento deletado com sucesso.");
        }
    }
}