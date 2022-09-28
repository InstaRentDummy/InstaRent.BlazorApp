using InstaRent.BlazorApp.Shared.Bags;
using InstaRent.BlazorApp.Shared.Dto;
using InstaRent.Catalog.Bags;
using InstaRent.Catalog.DailyClicks;
using InstaRent.Catalog.TotalClicks;
using System.Net.Http.Json;
using Volo.Abp.Application.Dtos;

namespace InstaRent.BlazorApp.Services.Catalog
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _http;
        string _url = "api/catalog";
        private PageParameters _pageParameters = new PageParameters() { PageSize = 10 };
        public event Action OnChange;

        public CatalogListDto Bags { get; set; }

        public CatalogService(HttpClient http)
        {
            _http = http;
        }

        public async Task IncreaseViewCount(string bag_id)
        {
            try
            {
                string _url = "";
                _url = $"api/catalog/increasecount/{bag_id}";
                var incValue = new HttpResponseMessage();
                await _http.PostAsJsonAsync<HttpResponseMessage>(_url, incValue);
            }
            catch
            {

            }

        }
        public async Task LoadCatalogs(string categoryType = null, string userId = "", string filterText = "")
        {

            int currentPage = 1;
            int _skipcount = _pageParameters.PageSize * (currentPage - 1);
            int _ttlcount = 0;
            string _url = "";

            switch (categoryType)
            {
                case "Popular":
                    {
                        _url = $"api/catalog/most-visited?FilterText={filterText}&SkipCount={_skipcount}&MaxResultCount={_pageParameters.PageSize}";
                        var response = await _http.GetFromJsonAsync<PagedResultDto<TotalClickWithNavigationPropertiesDto>>(_url);
                        if (response != null)
                        {
                            Bags.Items = response.Items.Select(c => c.Bag).ToList();
                            _ttlcount = (int)response.TotalCount;
                        }

                        break;
                    }
                case "Recommend for you":
                    {
                        _url = $"api/catalog/recommendation?userId={userId}&SkipCount={_skipcount}&MaxResultCount={_pageParameters.PageSize}";
                        var response = await _http.GetFromJsonAsync<PagedResultDto<BagDto>>(_url);
                        if (response != null)
                        {
                            Bags.Items = response.Items.ToList();
                            _ttlcount = (int)response.TotalCount;
                        }
                        break;
                    }
                default:
                    {
                        _url = $"api/catalog/trending?FilterText={filterText}&SkipCount={_skipcount}&MaxResultCount={_pageParameters.PageSize}";
                        var response = await _http.GetFromJsonAsync<PagedResultDto<DailyClickWithNavigationPropertiesDto>>(_url);
                        if (response != null)
                        {
                            List<BagDto> _bags = new List<BagDto>();
                            var bag = response.Items.ToList();
                            bag.ForEach(x => _bags.Add(x.Bag));
                            Bags = new CatalogListDto();
                            Bags.Items = _bags;
                            _ttlcount = (int)response.TotalCount;
                        }
                        break;
                    }
            }




            Bags.Meta = new MetaData()
            {
                CurrentPage = currentPage,
                PageSize = _pageParameters.PageSize,
                TotalCount = (int)_ttlcount,
                TotalPages = (int)_ttlcount / _pageParameters.PageSize
            };
        }

        public async Task<BagDto?> GetbyId(string id)
        {

            _url = $"api/catalog/bags/{id}";
            var bagdetail = await _http.GetFromJsonAsync<BagDto>(_url);
            return bagdetail;
        }

    }
}

