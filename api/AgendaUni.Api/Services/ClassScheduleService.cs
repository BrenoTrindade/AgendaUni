using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using AgendaUni.Api.Services.Interfaces;

namespace AgendaUni.Api.Services
{
    public class ClassScheduleService : IClassScheduleService
    {
        private readonly IClassScheduleRepository _classScheduleRepository;

        public ClassScheduleService(IClassScheduleRepository classScheduleRepository)
        {
            _classScheduleRepository = classScheduleRepository;
        }

        public async Task AddClassSchedule(ClassSchedule classSchedule)
        {
            await _classScheduleRepository.Add(classSchedule);
        }

        public async Task DeleteClassSchedule(int id)
        {
            var classSchedule = await _classScheduleRepository.GetById(id);
            if (classSchedule != null)
            {
                _classScheduleRepository.Delete(classSchedule);
            }
        }

        public async Task<IEnumerable<ClassSchedule>> GetAllClassSchedules()
        {
            return await _classScheduleRepository.GetAll();
        }

        public async Task<ClassSchedule> GetClassScheduleById(int id)
        {
            return await _classScheduleRepository.GetById(id);
        }

        public async Task UpdateClassSchedule(ClassSchedule classSchedule)
        {
            _classScheduleRepository.Update(classSchedule);
        }
    }
}