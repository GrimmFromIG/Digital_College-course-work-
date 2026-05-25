using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DigitalCollege.UI;
using DigitalCollege.UI.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<DigitalCollege.UI.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5008") });

builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
