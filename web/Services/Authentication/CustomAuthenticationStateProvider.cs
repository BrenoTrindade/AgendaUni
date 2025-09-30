using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net.Http.Headers;

namespace AgendaUni.Web.Services.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly JwtHelper _jwtHelper;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient, JwtHelper jwtHelper)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _jwtHelper = jwtHelper;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authToken = await _localStorage.GetItemAsStringAsync("authToken");

            var identity = new ClaimsIdentity();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    var jwt = _jwtHelper.GetTokenExpiration(authToken);

                    if (jwt.HasValue && jwt.Value < DateTime.UtcNow)
                        MarkUserAsLoggedOut();
                    else
                    {
                        identity = new ClaimsIdentity(_jwtHelper.ParseClaimsFromJwt(authToken), "jwt");
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                    }
                }
                catch
                {
                    await _localStorage.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(_jwtHelper.ParseClaimsFromJwt(token), "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}