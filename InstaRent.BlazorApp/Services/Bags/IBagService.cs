using InstaRent.BlazorApp.Shared.Bags;

namespace InstaRent.BlazorApp.Services.Bags
{
    public interface IBagService
    {
        BagListDto Bags { get; set; }
        Task GetListByUserId(int currentPage, string userId, string name, string description, string tags, string status);
        Task<BagInfoDto> GetInfoById(string bagId);
        Task<HttpResponseMessage> Create(BagInfoDto BagDto);
        Task<HttpResponseMessage> Update(BagInfoDto BagDto);
        Task<HttpResponseMessage> Delete(string bagId);
    }
}
