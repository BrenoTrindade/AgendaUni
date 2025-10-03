using AgendaUni.Common;
using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;

namespace AgendaUni.Services
{
    public class ClassScheduleService
    {
        private readonly IClassScheduleRepository _classScheduleRepository;

        public ClassScheduleService(IClassScheduleRepository classScheduleRepository)
        {
            _classScheduleRepository = classScheduleRepository;
        }

        public async Task<ServiceResult> AddClassScheduleAsync(ClassSchedule classSchedule)
        {
            if (classSchedule.ClassId == 0)
                return ServiceResult.Failure("Selecione uma aula.");

            if (!Enum.IsDefined(typeof(DayOfWeek), classSchedule.DayOfWeek))
                return ServiceResult.Failure("Selecione um dia válido da semana.");

            if (classSchedule.ClassTime == default)
                return ServiceResult.Failure("Informe o horário da aula.");

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


        public async Task<ServiceResult> UpdateClassScheduleAsync(ClassSchedule classSchedule)
        {
            if (classSchedule.ClassId == 0)
                return ServiceResult.Failure("Selecione uma aula.");

            if (!Enum.IsDefined(typeof(DayOfWeek), classSchedule.DayOfWeek))
                return ServiceResult.Failure("Selecione um dia válido da semana.");

            if (classSchedule.ClassTime == default)
                return ServiceResult.Failure("Informe o horário da aula.");

            await _classScheduleRepository.UpdateAsync(classSchedule);

            return ServiceResult.Success("Horário da aula atualizado com sucesso.");
        }

        public async Task<ServiceResult> DeleteClassScheduleAsync(int id)
        {
            var scheduleToDelete = await _classScheduleRepository.GetByIdAsync(id);
            if (scheduleToDelete == null)
                return ServiceResult.Failure("Horário não encontrado.");

            await _classScheduleRepository.DeleteAsync(id);

            return ServiceResult.Success("Horário deletado com sucesso.");
        }
    }
}