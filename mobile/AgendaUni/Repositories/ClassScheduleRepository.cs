using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;

namespace AgendaUni.Repositories
{
    public class ClassScheduleRepository : IClassScheduleRepository
    {
        private readonly AppDbContext _context;

        public ClassScheduleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ClassSchedule classObj)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO ClassSchedule (ClassId, DayOfWeek, ClassTime)
                VALUES ($classId, $dayOfWeek, $classTime);";

            command.Parameters.AddWithValue("$classId", classObj.ClassId);
            command.Parameters.AddWithValue("$dayOfWeek", classObj.DayOfWeek);
            command.Parameters.AddWithValue("classTime", classObj.ClassTime);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<ClassSchedule>> GetAllAsync()
        {
            var schedules = new List<ClassSchedule>();
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, ClassId, DayOfWeek, ClassTime FROM ClassSchedule;";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                schedules.Add(new ClassSchedule
                {
                    Id = reader.GetInt32(0),
                    ClassId = reader.GetInt32(1),
                    DayOfWeek = (DayOfWeek)reader.GetInt32(2),
                    ClassTime = reader.GetTimeSpan(3)
                });
            }
            return schedules;
        }
    }
}
