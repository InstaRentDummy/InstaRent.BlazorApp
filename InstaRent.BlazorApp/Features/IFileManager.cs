namespace InstaRent.BlazorApp.Features
{
    public interface IFileManager
    {
        public Task<FileInfoModel> Upload(Stream stream, string fileName, string container);
        FileInfoModel GetUri(string imageName, string container);
        Task<byte[]> GetFile(string imageName, string container);
        Task Delete(string imageName, string container);
    }
}
