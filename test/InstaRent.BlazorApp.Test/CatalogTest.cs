using Blazored.LocalStorage;
using InstaRent.BlazorApp.Pages.Catalog;
using InstaRent.BlazorApp.Services.Catalog;
using InstaRent.BlazorApp.Shared.Bags;
using Moq;
using System;
using System.Collections.Generic;

namespace InstaRent.BlazorApp.Test
{
    public class CatalogTest
    {
        private Bunit.TestContext testContext;
        private Mock<ICatalogService> CatalogService;
        private Mock<ILocalStorageService> LocalStorageService;

        [SetUp]
        public void Setup()
        {
            testContext = new Bunit.TestContext();

            CatalogService = new Mock<ICatalogService>();
            LocalStorageService = new Mock<ILocalStorageService>();

        }


        [TearDown]
        public void Teardown()
        {
            testContext.Dispose();
        }


        [Test]
        public void Listing()
        {
            testContext.Services.AddScoped(x => CatalogService.Object);
            testContext.Services.AddScoped(x => LocalStorageService.Object);

            var _bags = new List<BagInfoDto>()
            {
                new Shared.Bags.BagInfoDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    BagName = "New Bag",
                    Description = "New Bag Description",
                    ImageUrls = new List<string>() { "http://url.image.com/image1" },
                    RentalStartDate = new DateTime(2022, 6, 27),
                    RentalEndDate = new DateTime(2022, 9, 15),
                    Tags = new List<string>() { "Tote" },
                    Status = "available",
                    RenterId = "renter_1@gmail.com",
                    AvgRating = -1
                }
            };

            var _meta = new Shared.Dto.MetaData()
            {
                CurrentPage = 1,
                PageSize = 1,
                TotalCount = 1,
                TotalPages = 1,
            };

            

            var component = testContext.RenderComponent<CatalogList>(parameters => parameters
                .Add(p => p.Bags, _bags)
              );

            //component.SetParametersAndRender(parameters => parameters
            //  .Add(p => p.Bags, _bags)
            //  .Add(p => p.MetaData, _meta)
            //);
            //component.Render();

            var bags = component.FindAll("div.card");
            Assert.AreNotEqual(0, bags.Count);
        }

    }
}
