using BlazorDateRangePicker;
using InstaRent.BlazorApp.Shared.Bags;
using InstaRent.BlazorApp.Shared.Dto;
using InstaRent.Cart.Services;
using InstaRent.Catalog.DailyClicks;
using InstaRent.Catalog.TotalClicks;
using InstaRent.Catalog.UserPreferences;
using InstaRent.Payment.Transactions;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Volo.Abp.Application.Dtos;
using BagDto = InstaRent.Catalog.Bags.BagDto;

namespace InstaRent.BlazorApp.Services.Catalog
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _http;
        string _url = "api/catalog";
        private PageParameters _pageParameters = new PageParameters() { PageSize = 8 };
        public event Action OnAdd;

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

        public async Task LoadCatalogs(int currentPage, string categoryType = null, string userId = "", string filterText = "")
        {
            //int currentPage = 1;
            int _skipcount = _pageParameters.PageSize * (currentPage - 1);
            int _ttlcount = 0;
            string _url = "";
            Bags = new CatalogListDto();


            switch (categoryType)
            {
                case "Popular":
                    {
                        _url = $"api/catalog/most-visited?FilterText={filterText}&SkipCount={_skipcount}&MaxResultCount={_pageParameters.PageSize}";
                        var response = await _http.GetFromJsonAsync<PagedResultDto<TotalClickWithNavigationPropertiesDto>>(_url);
                        if (response != null)
                        {
                            Bags = new CatalogListDto();
                            Bags.Items = response.Items.Select(c => ConvertInfo(c.Bag)).ToList();
                            _ttlcount = (int)response.TotalCount;
                        }

                        break;
                    }
                case "Recommend for you":
                    {
                        if (userId != null)
                            _url = $"api/catalog/recommendations?UserId={userId}&SkipCount={_skipcount}&MaxResultCount={_pageParameters.PageSize}";
                        else
                            _url = $"api/catalog/trending?FilterText={filterText}&SkipCount={_skipcount}&MaxResultCount={_pageParameters.PageSize}";

                        var response = await _http.GetFromJsonAsync<PagedResultDto<BagDto>>(_url);
                        if (response != null)
                        {
                            Bags = new CatalogListDto();
                            Bags.Items = response.Items.Select(c => ConvertInfo(c)).ToList();
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
                            Bags = new CatalogListDto();
                            Bags.Items = response.Items.Select(c => ConvertInfo(c.Bag)).ToList();
                            _ttlcount = (int)response.TotalCount;
                        }
                        break;
                    }
            }
            var resutDto = PagedList<BagInfoDto>.ToPagedList(Bags.Items, _ttlcount, currentPage, _pageParameters.PageSize);
            Bags.Meta = resutDto.MetaData;

            

        }

        public async Task<BagDto?> GetbyId(string id)
        {

            _url = $"api/catalog/bags/{id}";
            var bagdetail = await _http.GetFromJsonAsync<BagDto>(_url);
            return bagdetail;
        }

        public async Task AddToCartAsync(BagDto bag, DateRange rentDateRange, string loginuserEmail)
        {
            if (bag == null || rentDateRange == null || string.IsNullOrEmpty(loginuserEmail))
                return;
            AddBagDto newItem = new AddBagDto();
            newItem.BagId = bag.Id;
            newItem.BagName = bag.bag_name;
            newItem.Tags = bag.tags;
            newItem.StartDate = rentDateRange.Start.DateTime;
            newItem.EndDate = rentDateRange.End.DateTime;
            var noOfDays = (int)(rentDateRange.End - rentDateRange.Start).TotalDays;
            newItem.Price = bag.price * noOfDays;
            newItem.ImageUrls = bag.image_urls;
            newItem.RenterId = bag.renter_id;
            newItem.LesseeId = loginuserEmail;
            newItem.Count = 1;

            _url = $"api/cart/items";
            var response = await _http.PostAsJsonAsync<AddBagDto>(_url, newItem, CancellationToken.None);
            if (response != null)
            {
                var cartlist = await response.Content.ReadFromJsonAsync<CartDto>();
                OnAdd.Invoke();
            }

        }

        public async Task RemoveItemFromCartAsync(string bagId, string loginuserEmail)
        {
            if (string.IsNullOrEmpty(bagId) || string.IsNullOrEmpty(loginuserEmail))
                return;

            _url = $"api/cart/items";
            var response = await _http.DeleteAsync($"{_url}?BagId={bagId}&Count=1&LesseeId={loginuserEmail}", CancellationToken.None);
            if (response != null)
            {
                await response.Content.ReadFromJsonAsync<CartDto>();
                OnAdd.Invoke();
            }

        }

        public async Task RemoveAllCartAsync(CartDto cartDto, string loginuserEmail)
        {
            if (cartDto == null || string.IsNullOrEmpty(loginuserEmail))
                return;

            _url = $"api/cart/items";

            foreach (var item in cartDto.Items)
            {
                var response = await _http.DeleteAsync($"{_url}?BagId={item.BagId}&Count=1&LesseeId={loginuserEmail}", CancellationToken.None);

                if (!response.IsSuccessStatusCode)
                    return;
            }

            OnAdd.Invoke();
        }

        public async Task<int?> GetCartItemCountbyUserIdAsync(string lesseeId)
        {
            _url = $"api/cart/items/{lesseeId}";

            var cartlist = await _http.GetFromJsonAsync<CartDto>(_url);
            if (cartlist != null)
                return cartlist.Items.Count;

            return null;
        }

        public async Task<CartDto> GetCartItembyUserIdAsync(string lesseeId)
        {
            _url = $"api/cart/items/{lesseeId}";

            var cartlist = await _http.GetFromJsonAsync<CartDto>(_url);

            if (cartlist != null)
                return cartlist;

            return null;
        }

        public async Task<bool?> CheckExistsInCart(string lesseeId, string bagId)
        {
            _url = $"api/cart/items/{lesseeId}";

            var cartlist = await _http.GetFromJsonAsync<CartDto>(_url);
            if (cartlist != null)
            {
                foreach (var item in cartlist.Items)
                {
                    if (item.BagId.ToString() == bagId)
                        return true;
                }
            }

            return false;
        }

        public async Task<List<DateRange>> BlackOutDates(string bagId)
        {
            _url = $"api/payment/transaction?bag_id={bagId}";
            List<DateRange> rented = new List<DateRange>();
            var translist = await _http.GetFromJsonAsync<PagedResultDto<TransactionDto>>(_url);

            foreach (var cart in translist.Items)
            {
                foreach (var bag in cart.Cart_Items)
                {
                    if (bag.BagId.ToString() == bagId)
                    {

                        if (bag.EndDate >= DateTime.Now)
                        {
                            DateRange r = new DateRange();
                            if (bag.StartDate >= DateTime.Now)
                                r.Start = bag.StartDate;
                            else
                                r.Start = bag.EndDate;
                            r.End = bag.EndDate;
                            rented.Add(r);
                        }
                    }
                }
            }
            return rented;

        }

        public async Task<bool> CheckRentedDate(string bagId, DateRange rentDateRange)
        {
            _url = $"api/payment/transaction/checktransaction?bagid={bagId}&startDate={rentDateRange.Start.Date.ToString("yyyy-MM-dd")}&EndDate={rentDateRange.End.Date.ToString("yyyy-MM-dd")}";

            return await _http.GetFromJsonAsync<bool>(_url);


        }

        private BagInfoDto ConvertInfo(BagDto? dto)
        {
            BagInfoDto info = new();

            try
            {
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
                    info.AvgRating = dto.AvgRating == null ? 0 : Convert.ToInt32(dto.AvgRating);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return info;

        }

        public async Task AddSearchTagforUser(string uId, string tag)
        {
            UserPreferenceTagUpdateDto searchTag= new UserPreferenceTagUpdateDto();
            searchTag.UserId = uId;
            searchTag.Tags = new List<string>() { tag };
            _url = $"api/catalog/userpreferences/updatetag";
           
            var response = await _http.PostAsJsonAsync<UserPreferenceTagUpdateDto>(_url, searchTag, CancellationToken.None);
            

        }
    }
}

