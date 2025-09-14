using System.Net.Http.Json;
using AgendaUni.Web.Models;

namespace AgendaUni.Web.Services
{
    public class EventService
    {
        private readonly HttpClient _httpClient;

        public EventService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Event>?> GetEvents()
        {
            return await _httpClient.GetFromJsonAsync<List<Event>>("api/Event");
        }

        public async Task AddEvent(Event newEvent)
        {
            await _httpClient.PostAsJsonAsync("api/Event", newEvent);
        }

        public async Task UpdateEvent(Event updatedEvent)
        {
            await _httpClient.PutAsJsonAsync($"api/Event/{updatedEvent.Id}", updatedEvent);
        }

        public async Task DeleteEvent(int id)
        {
            await _httpClient.DeleteAsync($"api/Event/{id}");
        }
    }
}