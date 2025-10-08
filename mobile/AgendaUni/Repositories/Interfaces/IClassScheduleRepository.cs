using AgendaUni.Models;

namespace AgendaUni.Repositories.Interfaces
{
    public interface IClassScheduleRepository
    {
        Task AddAsync(ClassSchedule classSchedule);
        Task<IEnumerable<ClassSchedule>> GetAllAsync();
        Task<ClassSchedule> GetByIdAsync(int id);
        Task UpdateAsync(ClassSchedule classSchedule);
        Task DeleteAsync(int id);
        Task<IEnumerable<ClassSchedule>> GetSchedulesByClassIdAsync(int classId);
    }
}
