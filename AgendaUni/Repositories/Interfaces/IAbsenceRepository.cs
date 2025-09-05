using AgendaUni.Models;

namespace AgendaUni.Repositories.Interfaces
{
    public interface IAbsenceRepository
    {
        Task AddAsync(Absence classObj);
        Task<IEnumerable<Absence>> GetAllAsync();
    }
}
