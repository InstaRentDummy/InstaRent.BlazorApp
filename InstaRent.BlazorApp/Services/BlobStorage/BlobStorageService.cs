using Azure;
using Azure.Storage.Blobs;
using InstaRent.BlazorApp.Shared.Dto;
using Microsoft.AspNetCore.Components.Forms;

namespace InstaRent.BlazorApp.Services.BlobStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        BlobSettings _settings;
        AzureSasCredential _credential;
        BlobClient _blobClient;

        public BlobStorageService(BlobSettings settings)
        {
            _settings = settings;
            _credential = new AzureSasCredential(_settings.SASKey);
        }

        public async Task<string> Upload(IBrowserFile file)
        {
            try
            {
                var blobUri = new Uri($"https://{_settings.AccountName}.blob.core.windows.net/{_settings.ContainerName}/{file.Name}");
                _blobClient = new BlobClient(blobUri, _credential, new BlobClientOptions());

                var res = await _blobClient.UploadAsync(file.OpenReadStream(_settings.MaxSize), overwrite: true);

                return blobUri.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<bool> Delete(string bagUrl)
        {
            try
            {
                var blobUri = new Uri(bagUrl);
                _blobClient = new BlobClient(blobUri, _credential, new BlobClientOptions());

                var res = await _blobClient.DeleteAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
