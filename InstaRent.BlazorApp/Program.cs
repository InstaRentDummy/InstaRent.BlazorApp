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
builder.Services.AddDateRangePicker(config =>
{
    config.Attributes = new Dictionary<string, object>
                {
                    { "class", "form-control form-control-sm" }
                };
});

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
