using Microsoft.AspNetCore.Components.Forms;

namespace InstaRent.BlazorApp.Services.BlobStorage
{
    public interface IBlobStorageService
    {
        Task<string> Upload(IBrowserFile file);
        Task<bool> Delete(string bagUrl);
    }
}
