namespace InstaRent.BlazorApp.Shared.Dto
{
    public class BlobSettings
    {
        public string AccountName { get; set; }
        public string ContainerName { get; set; }
        public int MaxSize { get; set; }
        public string SASKey { get; set; }
    }
}
