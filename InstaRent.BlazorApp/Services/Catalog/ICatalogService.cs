using InstaRent.BlazorApp.Shared.Bags;
using InstaRent.Catalog.Bags;
using InstaRent.Catalog.DailyClicks;
using InstaRent.Catalog.TotalClicks;
using InstaRent.Catalog.UserPreferences;
using System.Net.Http.Json;

namespace InstaRent.BlazorApp.Services.Catalog
{
    public interface ICatalogService
    {
        public CatalogListDto Bags { get; set; } 
        Task LoadCatalogs(string categoryType = null, string userId = "", string filterText="");
        Task IncreaseViewCount(string bag_id);
        Task<BagDto?> GetbyId(string id);
    }
}
