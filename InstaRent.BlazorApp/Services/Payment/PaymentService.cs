using InstaRent.Cart.Services;
using InstaRent.Payment.Transactions;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace InstaRent.BlazorApp.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _http;
        string _url = "api/payment";

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
                return null;
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
