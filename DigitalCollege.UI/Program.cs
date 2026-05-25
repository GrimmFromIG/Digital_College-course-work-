using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DigitalCollege.UI;
using DigitalCollege.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<DigitalCollege.UI.App>("#app");//builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7238") });//https://localhost:5085

builder.Services.AddScoped<ApiClient>();
builder.Services.AddSingleton<AuthService>();

await builder.Build().RunAsync();