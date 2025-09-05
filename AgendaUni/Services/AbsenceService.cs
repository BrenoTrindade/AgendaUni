using AgendaUni.Common;
using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;

namespace AgendaUni.Services
{
    public class AbsenceService
    {
        private readonly IAbsenceRepository _absenceRepository;

        public AbsenceService(IAbsenceRepository absenceRepository)
        {
            _absenceRepository = absenceRepository;
        }

        public async Task<ServiceResult> RegisterAbsenceAsync(Absence absence)
        {
            if (absence.ClassId == 0)
                return ServiceResult.Failure("Selecione uma aula.");

            if (string.IsNullOrWhiteSpace(absence.AbsenceReason))
                return ServiceResult.Failure("Informe o motivo da falta.");

            await _absenceRepository.AddAsync(absence);

            return ServiceResult.Success("Falta registrada com sucesso.");
        }

        public async Task<IEnumerable<Absence>> GetAllAbsencesAsync()
        {
            return await _absenceRepository.GetAllAsync();
        }
    }
}
