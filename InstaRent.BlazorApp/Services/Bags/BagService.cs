using InstaRent.BlazorApp.Shared.Bags;
using InstaRent.BlazorApp.Shared.Dto;
using InstaRent.Catalog.Bags;
using System.Net.Http.Json;
using Volo.Abp.Application.Dtos;

namespace InstaRent.BlazorApp.Services.Bags
{
    public class BagService : IBagService
    {
        private readonly HttpClient _http;
        string _url = "api/catalog/bags";
        private PageParameters _pageParameters = new PageParameters() { PageSize = 10 };
        public event Action OnChange;

        public BagListDto Bags { get; set; } = new();

        public BagService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetListByUserId(int currentPage, string userId, string name, string description, string tags, string status)
        {
            string _userId = string.IsNullOrEmpty(userId) ? string.Empty : userId;
            int _skipcount = _pageParameters.PageSize * (currentPage - 1);

            var response = await _http.GetFromJsonAsync<PagedResultDto<BagDto>>($"{_url}?renter_id={_userId}&bag_name={name}&description={description}&tags={tags}&status={status}&isdeleted=false&SkipCount={_skipcount}&MaxResultCount={_pageParameters.PageSize}");

            Bags.Items = response.Items.ToList();
            Bags.Meta = new MetaData()
            {
                CurrentPage = currentPage,
                PageSize = _pageParameters.PageSize,
                TotalCount = (int)response.TotalCount,
                TotalPages = (int)response.TotalCount / _pageParameters.PageSize
            };

            //OnChange.Invoke();
        }

        public async Task<BagDto> GetInfoById(string bagId)
        {
            var _bagDto = await _http.GetFromJsonAsync<BagDto>($"{_url}/{bagId}");
            return _bagDto == null ? new() : _bagDto;
        }

        public async Task<HttpResponseMessage> Create(BagDto bag)
        {
            var dto = new BagCreateDto()
            {
                bag_name = bag.bag_name,
                description = bag.description,
                image_urls = bag.image_urls,
                price = bag.price,
                tags = bag.tags,
                renter_id = bag.renter_id,
                rental_start_date = bag.rental_start_date,
                rental_end_date = bag.rental_end_date,
                status = "available"
            };
            return await _http.PostAsJsonAsync<BagCreateDto>(_url, dto, CancellationToken.None);
            //NavigationManager.NavigateTo("/product/Index");
        }

        public async Task<HttpResponseMessage> Update(BagDto bag)
        {
            return await _http.PutAsJsonAsync<BagUpdateDto>($"{_url}/{bag.Id}", new BagUpdateDto()
            {
                bag_name = bag.bag_name,
                description = bag.description,
                image_urls = bag.image_urls,
                price = bag.price,
                tags = bag.tags,
                renter_id = bag.renter_id,
                rental_start_date = bag.rental_start_date,
                rental_end_date = bag.rental_end_date,
            });
        }

        public async Task<HttpResponseMessage> Delete(string bagId)
        {
            return await _http.DeleteAsync(_url + $"/{bagId}");
        }

    }
}
