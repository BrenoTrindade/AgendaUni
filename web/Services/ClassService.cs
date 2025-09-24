using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using AgendaUni.Web.Models;
using Blazored.LocalStorage;

namespace AgendaUni.Web.Services
{
    public class ClassService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ClassService(HttpClient httpClient, ILocalStorageService localStorage)
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

        public async Task<IEnumerable<Class>> GetAllClasses()
        {
            await AddJwtHeader();
            return await _httpClient.GetFromJsonAsync<IEnumerable<Class>>("api/class");
        }

        public async Task<Class> GetClassById(int id)
        {
            await AddJwtHeader();
            return await _httpClient.GetFromJsonAsync<Class>($"api/class/{id}");
        }

        public async Task AddClass(Class @class)
        {
            await AddJwtHeader();
            await _httpClient.PostAsJsonAsync("api/class", @class);
        }

        public async Task UpdateClass(Class @class)
        {
            await AddJwtHeader();
            await _httpClient.PutAsJsonAsync($"api/class/{@class.Id}", @class);
        }

        public async Task DeleteClass(int id)
        {
            await AddJwtHeader();
            await _httpClient.DeleteAsync($"api/class/{id}");
        }
    }
}