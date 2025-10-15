using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            await _context.Events.AddAsync(ev);
            await _context.SaveChangesAsync();

            if (notificationIds != null)
            {
                foreach (var notificationId in notificationIds)
                {
                    ev.EventNotifications.Add(new EventNotification { EventId = ev.Id, NotificationId = notificationId });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events.Include(e => e.EventNotifications).ToListAsync();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events.Include(e => e.EventNotifications).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(Event ev, IEnumerable<int> notificationIds)
        {
            var existingEvent = await _context.Events.Include(e => e.EventNotifications).FirstOrDefaultAsync(e => e.Id == ev.Id);

            if (existingEvent != null)
            {
                existingEvent.EventDate = ev.EventDate;
                existingEvent.Description = ev.Description;
                existingEvent.ClassId = ev.ClassId;

                existingEvent.EventNotifications.Clear();
                if (notificationIds != null)
                {
                    foreach (var notificationId in notificationIds)
                    {
                        existingEvent.EventNotifications.Add(new EventNotification { NotificationId = notificationId });
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetEventsByClassIdAsync(int classId)
        {
            return await _context.Events
                .Where(e => e.ClassId == classId)
                .Include(e => e.EventNotifications)
                .ToListAsync();
        }
    }
}