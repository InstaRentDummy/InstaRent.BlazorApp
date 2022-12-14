using BlazorDateRangePicker;
using InstaRent.BlazorApp.Shared.Bags;
using InstaRent.Cart.Services;
using InstaRent.Catalog.UserPreferences;
using BagDto = InstaRent.Catalog.Bags.BagDto;

namespace InstaRent.BlazorApp.Services.Catalog
{
    public interface ICatalogService
    {
        event Action OnAdd;
        public CatalogListDto Bags { get; set; }
        Task LoadCatalogs(int currentPage, string categoryType = null, string userId = "", string filterText = "");
        Task IncreaseViewCount(string bag_id);
        Task<BagDto?> GetbyId(string id);
        Task AddToCartAsync(BagDto bag, DateRange rentDateRange, string loginuserEmail);
        Task RemoveItemFromCartAsync(string bagId, string loginuserEmail);
        Task RemoveAllCartAsync(CartDto cartDto, string loginuserEmail);
        Task<int?> GetCartItemCountbyUserIdAsync(string lesseeId);
        Task<CartDto> GetCartItembyUserIdAsync(string lesseeId);
        Task<bool?> CheckExistsInCart(string lesseeId, string bagId);
        Task<List<DateRange>> BlackOutDates(string bagId);
        Task<bool> CheckRentedDate(string bagId, DateRange rentDateRange);
        Task AddSearchTagforUser(string uId,string tag);
    }
}
