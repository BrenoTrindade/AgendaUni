using AgendaUni.Api.Models;

namespace AgendaUni.Api.Services.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetAllClasses();
        Task<Class> GetClassById(int id);
        Task AddClass(Class @class);
        Task UpdateClass(Class @class);
        Task DeleteClass(int id);
    }
}