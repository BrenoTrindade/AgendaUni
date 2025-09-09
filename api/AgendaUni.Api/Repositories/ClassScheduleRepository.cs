using AgendaUni.Api.Data;
using AgendaUni.Api.Interfaces;
using AgendaUni.Api.Models;

namespace AgendaUni.Api.Repositories
{
    public class ClassScheduleRepository : GenericRepository<ClassSchedule>, IClassScheduleRepository
    {
        public ClassScheduleRepository(AppDbContext context) : base(context)
        {
        }
    }
}