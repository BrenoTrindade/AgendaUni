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

        public async Task AddAsync(Event ev, IEnumerable<int> notificationIds)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = @"
                INSERT INTO Event (EventDate, Description, ClassId)
                VALUES ($eventDate, $description, $classId);
                SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$eventDate", ev.EventDate);
            command.Parameters.AddWithValue("$description", ev.Description);
            command.Parameters.AddWithValue("$classId", ev.ClassId);

            var eventId = (long)await command.ExecuteScalarAsync();

            foreach (var notificationId in notificationIds)
            {
                var notificationCommand = connection.CreateCommand();
                notificationCommand.Transaction = transaction;
                notificationCommand.CommandText = @"
                    INSERT INTO EventNotification (EventId, NotificationId)
                    VALUES ($eventId, $notificationId);";
                notificationCommand.Parameters.AddWithValue("$eventId", eventId);
                notificationCommand.Parameters.AddWithValue("$notificationId", notificationId);
                await notificationCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
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
                var ev = new Event
                {
                    Id = reader.GetInt32(0),
                    EventDate = reader.GetDateTime(1),
                    Description = reader.GetString(2),
                    ClassId = reader.GetInt32(3)
                };

                var notificationCommand = connection.CreateCommand();
                notificationCommand.CommandText = @"SELECT NotificationId FROM EventNotification WHERE EventId = $eventId;";
                notificationCommand.Parameters.AddWithValue("$eventId", ev.Id);
                using var notificationReader = await notificationCommand.ExecuteReaderAsync();
                while (await notificationReader.ReadAsync())
                {
                    ev.NotificationIds.Add(new EventNotification { NotificationId = notificationReader.GetInt32(0) });
                }
                events.Add(ev);
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
                var ev = new Event
                {
                    Id = reader.GetInt32(0),
                    EventDate = reader.GetDateTime(1),
                    Description = reader.GetString(2),
                    ClassId = reader.GetInt32(3)
                };

                var notificationCommand = connection.CreateCommand();
                notificationCommand.CommandText = @"SELECT NotificationId FROM EventNotification WHERE EventId = $eventId;";
                notificationCommand.Parameters.AddWithValue("$eventId", ev.Id);
                using var notificationReader = await notificationCommand.ExecuteReaderAsync();
                while (await notificationReader.ReadAsync())
                {
                    ev.NotificationIds.Add(new EventNotification { NotificationId = notificationReader.GetInt32(0) });
                }
                return ev;
            }
            return null;
        }

        public async Task UpdateAsync(Event ev, IEnumerable<int> notificationIds)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            var command = connection.CreateCommand();
            command.Transaction = transaction;
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

            var deleteNotificationsCommand = connection.CreateCommand();
            deleteNotificationsCommand.Transaction = transaction;
            deleteNotificationsCommand.CommandText = "DELETE FROM EventNotification WHERE EventId = $eventId;";
            deleteNotificationsCommand.Parameters.AddWithValue("$eventId", ev.Id);
            await deleteNotificationsCommand.ExecuteNonQueryAsync();

            foreach (var notificationId in notificationIds)
            {
                var notificationCommand = connection.CreateCommand();
                notificationCommand.Transaction = transaction;
                notificationCommand.CommandText = @"
                    INSERT INTO EventNotification (EventId, NotificationId)
                    VALUES ($eventId, $notificationId);";
                notificationCommand.Parameters.AddWithValue("$eventId", ev.Id);
                notificationCommand.Parameters.AddWithValue("$notificationId", notificationId);
                await notificationCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            var deleteNotificationsCommand = connection.CreateCommand();
            deleteNotificationsCommand.Transaction = transaction;
            deleteNotificationsCommand.CommandText = "DELETE FROM EventNotification WHERE EventId = $id;";
            deleteNotificationsCommand.Parameters.AddWithValue("$id", id);
            await deleteNotificationsCommand.ExecuteNonQueryAsync();

            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = "DELETE FROM Event WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", id);
            await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
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
                var ev = new Event
                {
                    Id = reader.GetInt32(0),
                    EventDate = reader.GetDateTime(1),
                    Description = reader.GetString(2),
                    ClassId = reader.GetInt32(3)
                };

                var notificationCommand = connection.CreateCommand();
                notificationCommand.CommandText = @"SELECT NotificationId FROM EventNotification WHERE EventId = $eventId;";
                notificationCommand.Parameters.AddWithValue("$eventId", ev.Id);
                using var notificationReader = await notificationCommand.ExecuteReaderAsync();
                while (await notificationReader.ReadAsync())
                {
                    ev.NotificationIds.Add(new EventNotification { NotificationId = notificationReader.GetInt32(0) });
                }
                events.Add(ev);
            }
            return events;
        }
    }
}