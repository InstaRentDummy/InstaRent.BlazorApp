using Blazored.LocalStorage;
using InstaRent.BlazorApp.Pages;
using InstaRent.BlazorApp.Services.Users;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Linq;

namespace InstaRent.BlazorApp.Test
{
    public class LoginTest : TestContextWrapper
    {
        private Bunit.TestContext testContext;
        private Mock<IUserService> UserService;
        private Mock<ILocalStorageService> LocalStorageService;

        [SetUp]
        public void Setup()
        {
            testContext = new Bunit.TestContext();
            UserService = new Mock<IUserService>();
            LocalStorageService = new Mock<ILocalStorageService>();
            //testContext.Services.AddSingleton<IUserService>(new UserService(new HttpClient { BaseAddress = new Uri("https://localhost:44326/") }));
            //Services.AddSingleton<NavigationManager>(new TestNav());
            //Services.AddSingleton<FakeAuthenticationStateProvider>();
        }

        [TearDown]
        public void Teardown()
        {
            testContext.Dispose();
        }

        [Test]
        public void LoginByLessee1()
        {
            testContext.Services.AddScoped(x => UserService.Object);
            testContext.Services.AddScoped(x => LocalStorageService.Object);

            var component = testContext.RenderComponent<Login>();

            var userName = component.Find("#username");
            userName.Change("lessee1@gmail.com");

            var password = component.Find("#password");
            password.Change("123456");

            var buttons = component.FindAll("button");
            var submit = buttons.FirstOrDefault(x => x.OuterHtml.Contains("submit"));

            submit.Click();
            var alert = component.Find("div.alert");
            Assert.AreEqual("Invalid Email or Password", alert.InnerHtml);
        }

    }

    public class LocalStorageServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return new DummyService();
        }
    }

    public class DummyService
    {

    }

    internal class TestNav : NavigationManager
    {
        public TestNav()
        {
            Initialize("https://unit-test.example/", "https://unit-test.example/");
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            NotifyLocationChanged(false);
        }
    }
}
