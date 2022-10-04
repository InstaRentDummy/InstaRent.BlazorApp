using InstaRent.BlazorApp.Shared.Transactions;
using InstaRent.Cart.Services;
using InstaRent.Payment.Transactions;

namespace InstaRent.BlazorApp.Services.Payment
{
    public interface IPaymentService
    {
        TransactionListDto Transactions { get; set; }
        Task<TransactionDto?> PaymentOrder(CartDto cartDto, string lesseeId);
        Task GetListByUserId(int currentPage, string userId, DateTime stratDate, DateTime endDate, bool isLessee);
    }
}
