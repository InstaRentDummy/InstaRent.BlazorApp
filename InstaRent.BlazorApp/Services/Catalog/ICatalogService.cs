using BlazorDateRangePicker;
using InstaRent.BlazorApp.Shared.Bags;
using InstaRent.Cart.Services;
using BagDto = InstaRent.Catalog.Bags.BagDto;

namespace InstaRent.BlazorApp.Services.Catalog
{
    public interface ICatalogService
    {
        event Action OnAdd;
        public CatalogListDto Bags { get; set; } 
        Task LoadCatalogs(string categoryType = null, string userId = "", string filterText="");
        Task IncreaseViewCount(string bag_id);
        Task<BagDto?> GetbyId(string id);
        Task AddToCartAsync(BagDto bag, DateRange rentDateRange, string loginuserEmail);
        Task<int?> GetCartItemCountbyUserIdAsync(string lesseeId);
        Task<bool?> CheckExistsInCart(string lesseeId, string bagId);
    }
}
