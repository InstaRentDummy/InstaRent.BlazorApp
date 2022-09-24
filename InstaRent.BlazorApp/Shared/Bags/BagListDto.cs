using InstaRent.BlazorApp.Shared.Dto;
using InstaRent.Catalog.Bags;

namespace InstaRent.BlazorApp.Shared.Bags
{
    public class BagListDto
    {
        public List<BagDto> Items { get; set; }
        public MetaData Meta { get; set; }
    }
}
