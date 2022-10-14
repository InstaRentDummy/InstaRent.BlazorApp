using InstaRent.BlazorApp.Shared.Dto;
using System.ComponentModel.DataAnnotations;

namespace InstaRent.BlazorApp.Shared.Bags
{
    public class BagListDto
    {
        public List<BagInfoDto> Items { get; set; }
        public MetaData Meta { get; set; }
    }

    public class CatalogListDto
    {
        public List<BagInfoDto> Items { get; set; }
        public MetaData Meta { get; set; }
    }

    public class BagInfoDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string BagName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public List<string> ImageUrls { get; set; }

        public DateTime RentalStartDate { get; set; }

        public DateTime RentalEndDate { get; set; }

        public double Price { get; set; }

        public List<string> Tags { get; set; }

        public string Status { get; set; }

        public string RenterId { get; set; }

        public int AvgRating { get; set; }
    }
}
