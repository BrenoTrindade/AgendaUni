using AgendaUni.Common;
using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;

namespace AgendaUni.Services
{
    public class ClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<List<Class>> GetAllClassesAsync()
        {
            return await _classRepository.GetAllAsync();
        }

        public async Task<Class> GetClassByIdAsync(int id)
        {
            return await _classRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> AddClassAsync(Class classObj)
        {
            if (string.IsNullOrWhiteSpace(classObj.ClassName))
                return ServiceResult.Failure("Informe o nome da aula.");
            
            if (classObj.MaximumAbsences < 0)
                return ServiceResult.Failure("Informe a quantidade de faltas.");

            await _classRepository.AddAsync(classObj);

            return ServiceResult.Success("Aula registrada com sucesso.");
        }
    }
}