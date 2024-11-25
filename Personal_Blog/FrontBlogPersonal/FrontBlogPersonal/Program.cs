using Blazored.LocalStorage;
using FrontBlogPersonal;
using FrontBlogPersonal.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var urlApi = builder.Configuration.GetValue<string>("urlApi");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(urlApi!) });
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
