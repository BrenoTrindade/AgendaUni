using AgendaUni.Common;
using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;

namespace AgendaUni.Services
{
    public class ClassScheduleService
    {
        private readonly IClassScheduleRepository _classScheduleRepository;
        private readonly IClassRepository _classRepository;
        private readonly NotificationService _notificationService;

        public ClassScheduleService(IClassScheduleRepository classScheduleRepository,IClassRepository classRepository ,NotificationService notificationService)
        {
            _classScheduleRepository = classScheduleRepository;
            _classRepository = classRepository;
            _notificationService = notificationService;
        }

        public async Task<ServiceResult> AddClassScheduleAsync(ClassSchedule classSchedule)
        {
            if (classSchedule.ClassId == 0)
                return ServiceResult.Failure("Selecione uma aula.");

            if (!Enum.IsDefined(typeof(DayOfWeek), classSchedule.DayOfWeek))
                return ServiceResult.Failure("Selecione um dia válido da semana.");

            if (classSchedule.ClassTime == default)
                return ServiceResult.Failure("Informe o horário da aula.");

            var parentClass = await _classRepository.GetByIdAsync(classSchedule.ClassId);
            if (parentClass == null)
            {
                return ServiceResult.Failure("A aula associada a este horário não foi encontrada.");
            }

            var notificationId = await _notificationService.ScheduleNotificationForClassSchedule(classSchedule, parentClass);
            classSchedule.NotificationId = notificationId;

            await _classScheduleRepository.AddAsync(classSchedule);

            return ServiceResult.Success("Horário da aula registrado com sucesso.");
        }

        public async Task<IEnumerable<ClassSchedule>> GetAllClassSchedulesAsync()
        {
            return await _classScheduleRepository.GetAllAsync();
        }

        public async Task<ClassSchedule> GetClassScheduleByIdAsync(int id)
        {
            return await _classScheduleRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ClassSchedule>> GetSchedulesByClassIdAsync(int classId)
        {
            return await _classScheduleRepository.GetSchedulesByClassIdAsync(classId);
        }


        public async Task<ServiceResult> UpdateClassScheduleAsync(ClassSchedule classSchedule)
        {
            if (classSchedule.ClassId == 0)
                return ServiceResult.Failure("Selecione uma aula.");

            if (!Enum.IsDefined(typeof(DayOfWeek), classSchedule.DayOfWeek))
                return ServiceResult.Failure("Selecione um dia válido da semana.");

            if (classSchedule.ClassTime == default)
                return ServiceResult.Failure("Informe o horário da aula.");

            var existingSchedule = await _classScheduleRepository.GetByIdAsync(classSchedule.Id);
            if (existingSchedule?.NotificationId != null)
            {
                _notificationService.CancelNotification(existingSchedule.NotificationId.Value);
            }

            var parentClass = await _classRepository.GetByIdAsync(classSchedule.ClassId);
            if (parentClass == null)
            {
                return ServiceResult.Failure("A aula associada a este horário não foi encontrada.");
            }
            var notificationId = await _notificationService.ScheduleNotificationForClassSchedule(classSchedule, parentClass);
            classSchedule.NotificationId = notificationId;

            await _classScheduleRepository.UpdateAsync(classSchedule);

            return ServiceResult.Success("Horário da aula atualizado com sucesso.");
        }

        public async Task<ServiceResult> DeleteClassScheduleAsync(int id)
        {
            var scheduleToDelete = await _classScheduleRepository.GetByIdAsync(id);
            if (scheduleToDelete == null)
                return ServiceResult.Failure("Horário não encontrado.");

            if (scheduleToDelete.NotificationId != null)
            {
                _notificationService.CancelNotification(scheduleToDelete.NotificationId.Value);
            }

            await _classScheduleRepository.DeleteAsync(id);

            return ServiceResult.Success("Horário deletado com sucesso.");
        }
    }
}