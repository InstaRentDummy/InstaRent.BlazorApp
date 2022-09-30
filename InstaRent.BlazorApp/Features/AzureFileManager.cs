using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace InstaRent.BlazorApp.Features
{
    public class AzureFileManager : IFileManager
    {
        private readonly BlobServiceClient _blobServiceClient;
        public AzureFileManager(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        async Task<FileInfoModel> IFileManager.Upload(Stream stream, string fileName, string container)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container.ToLower());

            //await blobContainer.CreateIfNotExistsAsync();
            //blobContainer.SetAccessPolicy(PublicAccessType.Blob);

            var blobClient = blobContainer.GetBlobClient(fileName.ToLower());

            await blobClient.UploadAsync(stream, overwrite: true);

            return new FileInfoModel { FileUri = blobClient.Uri, Filename = blobClient.Name };
        }

        public FileInfoModel GetUri(string imageName, string container)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container.ToLower());

            var blobClient = blobContainer.GetBlobClient(imageName.ToLower());
            return new FileInfoModel { FileUri = blobClient.Uri, Filename = blobClient.Name };
        }

        async Task<byte[]> IFileManager.GetFile(string imageName, string container)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container.ToLower());

            var blobClient = blobContainer.GetBlobClient(imageName.ToLower());
            var downloadContent = await blobClient.DownloadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        async Task IFileManager.Delete(string imageName, string container)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container.ToLower());

            var blobClient = blobContainer.GetBlobClient(imageName.ToLower());

            await blobClient.DeleteAsync();
        }


    }

    public class FileInfoModel
    {
        public Uri FileUri { get; set; }
        public string Filename { get; set; }
    }

    public class FileModel
    {
        public IFormFile ImageFile { get; set; }
    }
}