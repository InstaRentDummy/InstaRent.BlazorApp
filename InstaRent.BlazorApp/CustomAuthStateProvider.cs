using Blazored.LocalStorage;
using InstaRent.Login.Users;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace InstaRent.BlazorApp
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new ClaimsPrincipal());

            UserDto localUser = await _localStorage.GetItemAsync<UserDto>("user");

            if (localUser != null)
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, localUser.Name)
                }, "test authentication type");

                state = new AuthenticationState(new ClaimsPrincipal(identity));
            }
             

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
    }
}
