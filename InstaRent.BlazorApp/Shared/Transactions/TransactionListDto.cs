using InstaRent.BlazorApp.Shared.Dto;
using InstaRent.Payment.CartItems;
using InstaRent.Payment.Transactions;

namespace InstaRent.BlazorApp.Shared.Transactions
{
    public class TransactionListDto
    {
        public List<TransactionDto> Items { get; set; }
        public MetaData Meta { get; set; }
    }

    public class SearchInfo
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BagRating
    {
        public CartItemDto CartItem { get; set; }
        public int RatingNo { get; set; }
    }


}
