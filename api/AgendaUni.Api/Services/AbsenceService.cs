using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using AgendaUni.Api.Services.Interfaces;

namespace AgendaUni.Api.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IAbsenceRepository _absenceRepository;

        public AbsenceService(IAbsenceRepository absenceRepository)
        {
            _absenceRepository = absenceRepository;
        }

        public async Task AddAbsence(Absence absence)
        {
            await _absenceRepository.Add(absence);
            await _absenceRepository.Save();
        }

        public async Task DeleteAbsence(int id)
        {
            var absence = await _absenceRepository.GetById(id);
            if (absence != null)
            {
                _absenceRepository.Delete(absence);
                await _absenceRepository.Save();
            }
        }

        public async Task<IEnumerable<Absence>> GetAllAbsences()
        {
            return await _absenceRepository.GetAll();
        }

        public async Task<Absence> GetAbsenceById(int id)
        {
            return await _absenceRepository.GetById(id);
        }

        public async Task UpdateAbsence(Absence absence)
        {
            _absenceRepository.Update(absence);
        }
    }
}