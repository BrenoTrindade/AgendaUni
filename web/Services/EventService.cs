using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using AgendaUni.Web.Models;
using Blazored.LocalStorage;

namespace AgendaUni.Web.Services
{
    public class EventService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public EventService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task AddJwtHeader()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Event>?> GetEvents()
        {
            await AddJwtHeader();
            return await _httpClient.GetFromJsonAsync<List<Event>>("api/Event");
        }
        public async Task<Event> GetEventById(int id)
        {
            await AddJwtHeader();
            return await _httpClient.GetFromJsonAsync<Event>($"api/Event/{id}");
        }

        public async Task AddEvent(Event newEvent)
        {
            await AddJwtHeader();
            await _httpClient.PostAsJsonAsync("api/Event", newEvent);
        }

        public async Task UpdateEvent(Event updatedEvent)
        {
            await AddJwtHeader();
            await _httpClient.PutAsJsonAsync($"api/Event/{updatedEvent.Id}", updatedEvent);
        }

        public async Task DeleteEvent(int id)
        {
            await AddJwtHeader();
            await _httpClient.DeleteAsync($"api/Event/{id}");
        }
    }
}