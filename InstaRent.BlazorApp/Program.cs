using BlazorDateRangePicker;
using Blazored.LocalStorage;
using InstaRent.BlazorApp;
using InstaRent.BlazorApp.Services.Bags;
using InstaRent.BlazorApp.Services.BlobStorage;
using InstaRent.BlazorApp.Services.Catalog;
using InstaRent.BlazorApp.Services.Payment;
using InstaRent.BlazorApp.Services.Users;
using InstaRent.BlazorApp.Shared.Dto;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


var baseURL = builder.Configuration.GetValue<string>("App:BaseUrl");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseURL) });
builder.Services.AddScoped<IBagService, BagService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddDateRangePicker(config =>
{
    config.Attributes = new Dictionary<string, object>
                {
                    { "class", "form-control form-control-sm" }
                };
});

builder.Services.AddScoped(_ =>
{
    return new BlobSettings()
    {
        AccountName = builder.Configuration.GetValue<string>("BlobSettings:AccountName"),
        ContainerName = builder.Configuration.GetValue<string>("BlobSettings:ContainerName"),
        MaxSize = builder.Configuration.GetValue<int>("BlobSettings:MaxSize"),
        SASKey = builder.Configuration.GetValue<string>("BlobSettings:SASKey")
    };
});
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

//builder.Services.AddOidcAuthentication(options =>
//{
//    // Configure your authentication provider options here.
//    // For more information, see https://aka.ms/blazor-standalone-auth
//    builder.Configuration.Bind("Local", options.ProviderOptions);
//});

await builder.Build().RunAsync();
