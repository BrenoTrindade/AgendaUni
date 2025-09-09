using AgendaUni.Api.Data;
using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;

namespace AgendaUni.Api.Repositories
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        public ClassRepository(AppDbContext context) : base(context)
        {
        }
    }
}