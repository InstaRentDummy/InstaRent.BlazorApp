using Azure.Storage.Blobs;
using Blazored.LocalStorage;
using InstaRent.BlazorApp;
using InstaRent.BlazorApp.Features;
using InstaRent.BlazorApp.Services.Bags;
using InstaRent.BlazorApp.Services.Users;
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
builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient(builder.Configuration.GetValue<string>("AzureBlobStorageSettings:Connection"));
});

builder.Services.AddScoped<IFileManager, AzureFileManager>();

//builder.Services.AddOidcAuthentication(options =>
//{
//    // Configure your authentication provider options here.
//    // For more information, see https://aka.ms/blazor-standalone-auth
//    builder.Configuration.Bind("Local", options.ProviderOptions);
//});

await builder.Build().RunAsync();
