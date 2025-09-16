using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AgendaUni.Web;
using AgendaUni.Web.Services;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//var appSettings = await new HttpClient().GetFromJsonAsync<App>();

var s = builder.Configuration["ApiBaseUrl"];

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:8080")});


builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<AuthService>();


await builder.Build().RunAsync();
