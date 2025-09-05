using AgendaUni.Models;

namespace AgendaUni.Repositories.Interfaces
{
    public interface IClassScheduleRepository
    {
        Task AddAsync(ClassSchedule classSchedule);
        Task<IEnumerable<ClassSchedule>> GetAllAsync();
    }
}
