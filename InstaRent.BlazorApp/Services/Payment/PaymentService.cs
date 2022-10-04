using InstaRent.BlazorApp.Shared.Dto;
using InstaRent.BlazorApp.Shared.Transactions;
using InstaRent.Cart.Services;
using InstaRent.Payment.Transactions;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Volo.Abp.Application.Dtos;

namespace InstaRent.BlazorApp.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _http;
        string _url = "api/payment";

        private PageParameters _pageParameters = new PageParameters() { PageSize = 5 };
        public TransactionListDto Transactions { get; set; } = new();

        public PaymentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<TransactionDto?> PaymentOrder(CartDto cartDto, string lesseeId)
        {
            var dto = new TransactionCreateDto()
            {
                Lessee_Id = lesseeId,
                Date_Transacted = DateTime.Now,
                Cart_Items = ConvertDtoList(cartDto.Items)
            };

            var temp = JsonConvert.SerializeObject(dto);
            var response = await _http.PostAsJsonAsync<TransactionCreateDto>($"{_url}/order", dto, CancellationToken.None);

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<TransactionDto>().Result;
            }
            else
            {
                var error = response.Content.ReadFromJsonAsync<ErrorResponseMessage>().Result;
                throw new Exception(error.Error.Message);
            }
        }

        public async Task GetListByUserId(int currentPage, string userId, DateTime stratDate, DateTime endDate, bool isLessee)
        {
            string _userId = string.IsNullOrEmpty(userId) ? string.Empty : userId;
            int _skipcount = _pageParameters.PageSize * (currentPage - 1);

            string _userInfo = isLessee ? $"lessee_id={_userId}" : $"renter_Id={_userId}";
            string _dateInfo = $"date_transactedMin={stratDate.ToString("yyyy-MM-dd")}&date_transactedMax={endDate.ToString("yyyy-MM-dd")}";

            var response = await _http.GetFromJsonAsync<PagedResultDto<TransactionDto>>($"{_url}/transaction?{_userInfo}&{_dateInfo}&isdeleted=false&SkipCount={_skipcount}&MaxResultCount={_pageParameters.PageSize}");
            var tansactionList = response == null ? new() : response.Items.ToList();

            var resutDto = PagedList<TransactionDto>.ToPagedList(tansactionList, (int)response.TotalCount, currentPage, _pageParameters.PageSize);

            Transactions.Items = tansactionList;
            Transactions.Meta = resutDto.MetaData;
        }


        private List<InstaRent.Payment.CartItems.CartItemDto> ConvertDtoList(List<CartItemDto> cartItems)
        {
            List<InstaRent.Payment.CartItems.CartItemDto> cartItemDtos = new();

            foreach (var item in cartItems)
            {
                cartItemDtos.Add(new InstaRent.Payment.CartItems.CartItemDto()
                {
                    BagId = item.BagId,
                    BagName = item.BagName,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    ImageUrls = new(),//item.ImageUrls,
                    Price = item.price,
                    RenterId = item.RenterId,
                    Tags = item.Tags
                });
            }

            return cartItemDtos;
        }

    }
}
