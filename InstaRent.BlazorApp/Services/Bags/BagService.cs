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
        private PageParameters _pageParameters = new PageParameters() { PageSize = 5 };
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
            var bagList = response.Items.Select(x => ConvertInfo(x)).ToList();

            var resutDto = PagedList<BagInfoDto>.ToPagedList(bagList, (int)response.TotalCount, currentPage, _pageParameters.PageSize);

            Bags.Items = bagList;
            Bags.Meta = resutDto.MetaData;

            //OnChange.Invoke();
        }

        public async Task<BagInfoDto> GetInfoById(string bagId)
        {
            var _bagDto = await _http.GetFromJsonAsync<BagDto>($"{_url}/{bagId}");
            return _bagDto == null ? new() : ConvertInfo(_bagDto);
        }

        public async Task<HttpResponseMessage> Create(BagInfoDto bag)
        {
            var dto = new BagCreateDto()
            {
                bag_name = bag.BagName,
                description = bag.Description,
                image_urls = bag.ImageUrls,
                price = bag.Price,
                tags = bag.Tags,
                renter_id = bag.RenterId,
                rental_start_date = bag.RentalStartDate,
                rental_end_date = bag.RentalEndDate,
                status = "available"
            };
            return await _http.PostAsJsonAsync<BagCreateDto>(_url, dto, CancellationToken.None);
            //NavigationManager.NavigateTo("/product/Index");
        }

        public async Task<HttpResponseMessage> Update(BagInfoDto bag)
        {
            return await _http.PutAsJsonAsync<BagUpdateDto>($"{_url}/{bag.Id}", new BagUpdateDto()
            {
                bag_name = bag.BagName,
                description = bag.Description,
                image_urls = bag.ImageUrls,
                price = bag.Price,
                tags = bag.Tags,
                renter_id = bag.RenterId,
                rental_start_date = bag.RentalStartDate,
                rental_end_date = bag.RentalEndDate,
            });
        }

        public async Task<HttpResponseMessage> Delete(string bagId)
        {
            return await _http.DeleteAsync(_url + $"/{bagId}");
        }

        private BagInfoDto ConvertInfo(BagDto? dto)
        {
            BagInfoDto info = new();

            if (dto != null)
            {
                info.Id = dto.Id.ToString();
                info.BagName = dto.bag_name;
                info.Description = dto.description;
                info.ImageUrls = dto.image_urls;
                info.Price = dto.price;
                info.Tags = dto.tags;
                info.RentalStartDate = dto.rental_start_date;
                info.RentalEndDate = dto.rental_end_date;
                info.Status = dto.status;
                info.RenterId = dto.renter_id;
            }

            return info;
        }

    }
}
