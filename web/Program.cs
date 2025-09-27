using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AgendaUni.Web;
using AgendaUni.Web.Services;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//var appSettings = await new HttpClient().GetFromJsonAsync<App>();

var s = builder.Configuration["ApiBaseUrl"];

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:8080")});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EventService>(sp => new EventService(sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<ILocalStorageService>()));
builder.Services.AddScoped<ClassService>(sp => new ClassService(sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<ILocalStorageService>()));
builder.Services.AddScoped<AbsenceService>(sp => new AbsenceService(sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<ILocalStorageService>()));

var culture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await builder.Build().RunAsync();
