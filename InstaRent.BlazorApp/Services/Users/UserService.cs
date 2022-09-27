using InstaRent.Login.Users;
using System.Net.Http.Json;

namespace InstaRent.BlazorApp.Services.Users
{
    public class UserService : IUserService
    {
        private readonly HttpClient _http;
        string _url = "api/user";

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<UserDto?> Login(LoginUserDto user)
        {
            var dto = new LoginUserDto()
            {
                Email = user.Email,
                Password = user.Password
            };

            var response = await _http.PostAsJsonAsync<LoginUserDto>($"{_url}/login", dto, CancellationToken.None);

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<UserDto>().Result;
            }
            else
                return null;
        }

        public async Task<UserDto> GetInfoById(string userId)
        {
            var _userDto = await _http.GetFromJsonAsync<UserDto>($"{_url}/{userId}");
            return _userDto == null ? new() : _userDto;
        }

        public async Task<HttpResponseMessage> Create(UserDto user)
        {
            var dto = new CreateUpdateUserDto()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };
            return await _http.PostAsJsonAsync<CreateUpdateUserDto>(_url, dto, CancellationToken.None);
            //NavigationManager.NavigateTo("/product/Index");
        }

        public async Task<HttpResponseMessage> Update(UserDto user)
        {
            return await _http.PutAsJsonAsync<CreateUpdateUserDto>($"{_url}/{user.Id}", new CreateUpdateUserDto()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            });
        }

    }
}
