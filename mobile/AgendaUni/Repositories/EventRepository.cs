using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;

namespace AgendaUni.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Event ev)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
        INSERT INTO Event (EventDate, Description, ClassId)
        VALUES ($eventDate, $description, $classId);
        SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$eventDate", ev.EventDate);
            command.Parameters.AddWithValue("$description", ev.Description);
            command.Parameters.AddWithValue("$classId", ev.ClassId);

            await command.ExecuteScalarAsync();
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var events = new List<Event>();
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, EventDate, Description, ClassId FROM Event;";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                events.Add(new Event
                {
                    Id = reader.GetInt32(0),
                    EventDate = reader.GetDateTime(1),
                    Description = reader.GetString(2),
                    ClassId = reader.GetInt32(3)
                });
            }
            return events;
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, EventDate, Description, ClassId FROM Event WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", id);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Event
                {
                    Id = reader.GetInt32(0),
                    EventDate = reader.GetDateTime(1),
                    Description = reader.GetString(2),
                    ClassId = reader.GetInt32(3)
                };
            }
            return null;
        }

        public async Task UpdateAsync(Event ev)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Event SET
                    EventDate = $eventDate,
                    Description = $description,
                    ClassId = $classId
                WHERE Id = $id;";
            command.Parameters.AddWithValue("$eventDate", ev.EventDate);
            command.Parameters.AddWithValue("$description", ev.Description);
            command.Parameters.AddWithValue("$classId", ev.ClassId);
            command.Parameters.AddWithValue("$id", ev.Id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Event WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByClassIdAsync(int classId)
        {
            var events = new List<Event>();
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, EventDate, Description, ClassId FROM Event WHERE ClassId = $classId;";
            command.Parameters.AddWithValue("$classId", classId);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                events.Add(new Event
                {
                    Id = reader.GetInt32(0),
                    EventDate = reader.GetDateTime(1),
                    Description = reader.GetString(2),
                    ClassId = reader.GetInt32(3)
                });
            }
            return events;
        }
    }
}