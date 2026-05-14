using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DigitalCollege.UI;
using DigitalCollege.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5085") });

builder.Services.AddScoped<ApiClient>();
builder.Services.AddSingleton<AuthService>();

await builder.Build().RunAsync();