using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;
using AgendaUni.Api.Services.Interfaces;

namespace AgendaUni.Api.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task AddClass(Class @class)
        {
            try
            {

                await _classRepository.Add(@class);
                await _classRepository.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteClass(int id)
        {
            var @class = await _classRepository.GetById(id);
            if (@class != null)
            {
                _classRepository.Delete(@class);
                await _classRepository.Save();
            }
        }

        public async Task<IEnumerable<Class>> GetAllClasses()
        {
            return await _classRepository.GetAll();
        }

        public async Task<Class> GetClassById(int id)
        {
            return await _classRepository.GetById(id);
        }

        public async Task UpdateClass(Class @class)
        {
            _classRepository.Update(@class);
        }
    }
}