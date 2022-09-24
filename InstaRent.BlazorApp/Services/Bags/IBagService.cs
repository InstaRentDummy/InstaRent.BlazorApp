using InstaRent.BlazorApp.Shared.Bags;
using InstaRent.Catalog.Bags;

namespace InstaRent.BlazorApp.Services.Bags
{
    public interface IBagService
    {
        event Action OnChange;
        BagListDto Bags { get; set; }
        Task GetListByUserId(int currentPage, string userId, string name, string description, string tags, string status);
        Task<BagDto> GetInfoById(string bagId);
        Task<HttpResponseMessage> Create(BagDto BagDto);
        Task<HttpResponseMessage> Update(BagDto BagDto);
        Task<HttpResponseMessage> Delete(string bagId);
    }
}
