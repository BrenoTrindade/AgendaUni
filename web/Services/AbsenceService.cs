using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using AgendaUni.Web.Models;
using Blazored.LocalStorage;

namespace AgendaUni.Web.Services
{
    public class AbsenceService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AbsenceService(HttpClient httpClient, ILocalStorageService localStorage)
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

        public async Task<IEnumerable<Absence>> GetAllAbsences()
        {
            await AddJwtHeader();
            return await _httpClient.GetFromJsonAsync<IEnumerable<Absence>>("api/absence");
        }

        public async Task<Absence> GetAbsenceById(int id)
        {
            await AddJwtHeader();
            return await _httpClient.GetFromJsonAsync<Absence>($"api/absence/{id}");
        }

        public async Task AddAbsence(Absence absence)
        {
            await AddJwtHeader();
            await _httpClient.PostAsJsonAsync("api/absence", absence);
        }

        public async Task UpdateAbsence(Absence absence)
        {
            await AddJwtHeader();
            await _httpClient.PutAsJsonAsync($"api/absence/{@absence.Id}", absence);
        }

        public async Task DeleteAbsence(int id)
        {
            await AddJwtHeader();
            await _httpClient.DeleteAsync($"api/absence/{id}");
        }
    }
}