using AgendaUni.Api.Models;

namespace AgendaUni.Api.Services.Interfaces
{
    public interface IClassScheduleService
    {
        Task<IEnumerable<ClassSchedule>> GetAllClassSchedules();
        Task<ClassSchedule> GetClassScheduleById(int id);
        Task AddClassSchedule(ClassSchedule classSchedule);
        Task UpdateClassSchedule(ClassSchedule classSchedule);
        Task DeleteClassSchedule(int id);
    }
}