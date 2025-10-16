using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
using Plugin.LocalNotification;

namespace AgendaUni.Services
{
    public class NotificationService
    {
        private readonly IClassRepository _classRepository;
        private readonly IClassScheduleRepository _classScheduleRepository;
        private readonly IEventRepository _eventRepository;

        public NotificationService(IClassRepository classRepository, IClassScheduleRepository classScheduleRepository, IEventRepository eventRepository)
        {
            _classRepository = classRepository;
            _classScheduleRepository = classScheduleRepository;
            _eventRepository = eventRepository;
        }

        public bool IsEventNotificationEnabled => Preferences.Get(nameof(IsEventNotificationEnabled), true);

        public bool IsClassScheduleNotificationEnabled => Preferences.Get(nameof(IsClassScheduleNotificationEnabled), true);

        public async Task<IList<NotificationRequest>> GetPendingNotificationRequests()
        {
            return await LocalNotificationCenter.Current.GetPendingNotificationList();
        }

        public async Task<List<int>> ScheduleNotificationForEvent(Event ev, Class cl)
        {
            if (!IsEventNotificationEnabled) return new List<int>();
            var notificationIds = new List<int>();

            var titleSemana = $"Evento em uma semana: {cl.ClassName}";
            var descSemana = $"Seu evento '{ev.Description}' será na próxima semana, no dia {ev.EventDate:dd/MM}.";
            var notifyTimeSemana = ev.EventDate.AddDays(-7).AddHours(6);
            var idSemana = await ScheduleNotification(titleSemana, descSemana, notifyTimeSemana, "event");
            if (idSemana != -1) notificationIds.Add(idSemana);

            var titleDia = $"Evento amanhã: {cl.ClassName}";
            var descDia = $"Seu evento '{ev.Description}' é amanhã.";
            var notifyTimeDia = ev.EventDate.AddDays(-1).AddHours(6);
            var idDia = await ScheduleNotification(titleDia, descDia, notifyTimeDia, "event");
            if (idDia != -1) notificationIds.Add(idDia);

            return notificationIds;
        }

        public async Task<int> ScheduleNotificationForClassSchedule(ClassSchedule sch, Class cl)
        {
            if (!IsClassScheduleNotificationEnabled) return -1;
            var now = DateTime.Now;
            var scheduleDayOfWeek = (int)sch.DayOfWeek;
            var currentDayOfWeek = (int)now.DayOfWeek;

            var daysToAdd = scheduleDayOfWeek - currentDayOfWeek;
            if (daysToAdd < 0)
            {
                daysToAdd += 7;
            }

            var nextClassDate = now.Date.AddDays(daysToAdd);
            var notificationTime = nextClassDate.Add(sch.ClassTime).AddHours(-1);

            if (notificationTime < now)
            {
                notificationTime = notificationTime.AddDays(7);
            }

            var request = new NotificationRequest
            {
                Title = "Lembrete de Aula",
                Description = $"Sua aula de {cl.ClassName} começa em 1 hora.",
                ReturningData = "class_schedule",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notificationTime,
                    RepeatType = NotificationRepeat.Weekly
                }
            };
            await LocalNotificationCenter.Current.Show(request);
            return request.NotificationId;
        }
        private async Task<int> ScheduleNotification(string title, string description, DateTime notifyTime, string returningData)
        {
            if (notifyTime > DateTime.Now)
            {
                var request = new NotificationRequest
                {
                    Title = title,
                    Description = description,
                    ReturningData = returningData,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = notifyTime
                    }
                };

                await LocalNotificationCenter.Current.Show(request);
                return request.NotificationId;
            }
            return -1;
        }

        public void CancelNotification(int notificationId)
        {
            LocalNotificationCenter.Current.Cancel(notificationId);
        }

        public async Task CancelNotificationsByType(string notificationType)
        {
            var pendingNotifications = await GetPendingNotificationRequests();
            foreach (var notification in pendingNotifications)
            {
                if (notification.ReturningData == notificationType)
                {
                    LocalNotificationCenter.Current.Cancel(notification.NotificationId);
                }
            }
        }

        public async Task RescheduleEventNotifications()
        {
            var classes = await _classRepository.GetAllAsync();
            foreach (var cl in classes)
            {
                var events = await _eventRepository.GetEventsByClassIdAsync(cl.Id);
                foreach (var ev in events)
                {
                    await ScheduleNotificationForEvent(ev, cl);
                }
            }
        }

        public async Task RescheduleClassScheduleNotifications()
        {
            var classes = await _classRepository.GetAllAsync();
            foreach (var cl in classes)
            {
                var schedules = await _classScheduleRepository.GetSchedulesByClassIdAsync(cl.Id);
                foreach (var schedule in schedules)
                {
                    await ScheduleNotificationForClassSchedule(schedule, cl);
                }
            }
        }
    }
}
