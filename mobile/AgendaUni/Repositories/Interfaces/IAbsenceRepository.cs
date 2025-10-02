using AgendaUni.Models;

namespace AgendaUni.Repositories.Interfaces
{
    public interface IAbsenceRepository
    {
        Task AddAsync(Absence absence);
        Task<IEnumerable<Absence>> GetAllAsync();
        Task<Absence> GetByIdAsync(int id);
        Task UpdateAsync(Absence absence);
        Task DeleteAsync(int id);
    }
}
