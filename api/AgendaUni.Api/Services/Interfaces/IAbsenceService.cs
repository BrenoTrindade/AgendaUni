using AgendaUni.Api.Models;

namespace AgendaUni.Api.Services.Interfaces
{
    public interface IAbsenceService
    {
        Task<IEnumerable<Absence>> GetAllAbsences();
        Task<Absence> GetAbsenceById(int id);
        Task AddAbsence(Absence absence);
        Task UpdateAbsence(Absence absence);
        Task DeleteAbsence(int id);
    }
}