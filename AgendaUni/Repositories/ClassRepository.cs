using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
namespace AgendaUni.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Class>> GetAllAsync()
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                SELECT 
                                    c.Id, c.ClassName, c.MaximumAbsences,
                                    a.Id AS AbsenceId, a.AbsenceDate, a.AbsenceReason,
                                    s.Id AS ScheduleId, s.DayOfWeek, s.ClassTime
                                FROM Class c
                                LEFT JOIN Absence a ON c.Id = a.ClassId
                                LEFT JOIN ClassSchedule s ON c.Id = s.ClassId
                                ORDER BY c.Id, s.DayOfWeek;";

            using var reader = await command.ExecuteReaderAsync();
            var classes = new List<Class>();
            var classDict = new Dictionary<int, Class>();

            while (await reader.ReadAsync())
            {
                int classId = reader.GetInt32(0);

                if (!classDict.TryGetValue(classId, out var classItem))
                {
                    classItem = new Class
                    {
                        Id = classId,
                        ClassName = reader.GetString(1),
                        MaximumAbsences = reader.GetInt32(2),
                        Absences = new List<Absence>(),
                        Schedules = new List<ClassSchedule>()
                    };

                    classDict[classId] = classItem;
                    classes.Add(classItem);
                }

                if (!reader.IsDBNull(3))
                {
                    classItem.Absences.Add(new Absence
                    {
                        Id = reader.GetInt32(3),
                        AbsenceDate = reader.GetDateTime(4),
                        AbsenceReason = reader.IsDBNull(5) ? null : reader.GetString(5),
                        ClassId = classId
                    });
                }

                if (!reader.IsDBNull(6))
                {
                    var scheduleId = reader.GetInt32(6);

                    bool alreadyExists = classItem.Schedules.Any(s => s.Id == scheduleId);
                    if (!alreadyExists)
                    {
                        classItem.Schedules.Add(new ClassSchedule
                        {
                            Id = scheduleId,
                            DayOfWeek = (DayOfWeek)reader.GetInt32(7),
                            ClassTime = reader.GetTimeSpan(8),
                            ClassId = classId
                        });
                    }
                }
            }

            return classes;
        }

        public async Task<Class> GetByIdAsync(int id)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, ClassName, MaximumAbsences FROM Class WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", id);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Class
                {
                    Id = reader.GetInt32(0),
                    ClassName = reader.GetString(1),
                    MaximumAbsences = reader.GetInt32(2)
                };
            }
            return null;
        }

        public async Task AddAsync(Class classObj)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Class (ClassName, MaximumAbsences)
                VALUES ($className, $maximumAbsences);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$className", classObj.ClassName);
            command.Parameters.AddWithValue("$maximumAbsences", classObj.MaximumAbsences);

            await command.ExecuteScalarAsync();
        }
    }
}
