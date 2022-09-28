using InstaRent.BlazorApp.Shared.Users;
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

        public async Task<UserInfoDto?> Login(UserLoginInfoDto user)
        {
            var dto = new LoginUserDto()
            {
                Email = user.Email,
                Password = user.Password
            };

            var response = await _http.PostAsJsonAsync<LoginUserDto>($"{_url}/login", dto, CancellationToken.None);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadFromJsonAsync<UserDto>().Result;

                return ConvertInfo(result);
            }
            else
                return null;
        }

        public async Task<UserInfoDto> GetInfoById(string userId)
        {
            var _user = await _http.GetFromJsonAsync<UserDto>($"{_url}/{userId}");
            return _user == null ? new() : ConvertInfo(_user);
        }

        public async Task<HttpResponseMessage> Create(UserInfoDto user)
        {
            var dto = new CreateUpdateUserDto()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };
            return await _http.PostAsJsonAsync<CreateUpdateUserDto>($"{_url}/Register", dto, CancellationToken.None);
            //NavigationManager.NavigateTo("/product/Index");
        }

        public async Task<HttpResponseMessage> Update(UserInfoDto user)
        {
            return await _http.PutAsJsonAsync<CreateUpdateUserDto>($"{_url}/{user.Id}", new CreateUpdateUserDto()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            });
        }

        private UserInfoDto ConvertInfo(UserDto? dto)
        {
            UserInfoDto info = new UserInfoDto();

            if (dto != null)
            {
                info.Id = dto.Id.ToString();
                info.Name = dto.Name;
                info.Email = dto.Email;
                info.Role = dto.Role;
            }

            return info;
        }
    }
}
