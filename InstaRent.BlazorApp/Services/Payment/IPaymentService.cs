using InstaRent.Cart.Services;
using InstaRent.Payment.Transactions;

namespace InstaRent.BlazorApp.Services.Payment
{
    public interface IPaymentService
    {
        Task<TransactionDto?> PaymentOrder(CartDto cartDto, string lesseeId);
    }
}
