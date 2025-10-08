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

        public async Task<ServiceResult<Class>> AddClassAsync(Class classObj)
        {
            if (string.IsNullOrWhiteSpace(classObj.ClassName))
                return (ServiceResult<Class>)ServiceResult.Failure("Informe o nome da aula.");
            
            if (classObj.MaximumAbsences < 0)
                return (ServiceResult<Class>)ServiceResult.Failure("Informe a quantidade de faltas.");

            var savedClass = await _classRepository.AddAsync(classObj);

            return ServiceResult<Class>.Success(savedClass, "Aula registrada com sucesso.");
        }

        public async Task<ServiceResult> UpdateClassAsync(Class classObj)
        {
            if (string.IsNullOrWhiteSpace(classObj.ClassName))
                return ServiceResult.Failure("O nome da aula não pode ser vazio.");

            if (classObj.MaximumAbsences < 0)
                return ServiceResult.Failure("A quantidade máxima de faltas não pode ser negativa.");

            await _classRepository.UpdateAsync(classObj);

            return ServiceResult.Success("Aula atualizada com sucesso.");
        }

        public async Task<ServiceResult> DeleteClassAsync(int id)
        {
            var classToDelete = await _classRepository.GetByIdAsync(id);
            if (classToDelete == null)
                return ServiceResult.Failure("Aula não encontrada.");

            await _classRepository.DeleteAsync(id);

            return ServiceResult.Success("Aula deletada com sucesso.");
        }
    }
}