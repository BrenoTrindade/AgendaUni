using AgendaUni.Api.Data;
using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;

namespace AgendaUni.Api.Repositories
{
    public class AbsenceRepository : GenericRepository<Absence>, IAbsenceRepository
    {
        public AbsenceRepository(AppDbContext context) : base(context)
        {
        }
    }
}