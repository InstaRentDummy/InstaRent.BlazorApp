namespace InstaRent.BlazorApp.Shared.Dto
{
    public class PagedResultDto<T>
    {
        public long TotalCount { get; set; }
        public IReadOnlyList<T> items { get; set; }
    }
}
