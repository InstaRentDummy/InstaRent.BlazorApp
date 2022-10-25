using Blazored.LocalStorage;
using InstaRent.BlazorApp.Pages;
using InstaRent.BlazorApp.Services.Users;
using InstaRent.BlazorApp.Shared.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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

            UserService.Setup(_ => _.Login(It.Is<UserLoginInfoDto>(x => x.Email.Equals("lessee1@gmail.com") && x.Password.Equals("123456"))))
            .ReturnsAsync(new Shared.Users.UserInfoDto());
            //testContext.Services.AddSingleton<IUserService>(new UserService(new HttpClient { BaseAddress = new Uri("https://localhost:44326/") }));
            testContext.Services.AddSingleton<NavigationManager>(new TestNav());
            testContext.Services.AddSingleton<AuthenticationStateProvider>(new CustomAuthStateProvider(LocalStorageService.Object));
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
            Assert.AreEqual(string.Empty, alert.InnerHtml);
        }


        [Test]
        public void ErrorLoginByLessee()
        {
            testContext.Services.AddScoped(x => UserService.Object);
            testContext.Services.AddScoped(x => LocalStorageService.Object);

            var component = testContext.RenderComponent<Login>();

            var userName = component.Find("#username");
            userName.Change("l1@gmail.com");

            var password = component.Find("#password");
            password.Change("111111");

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
