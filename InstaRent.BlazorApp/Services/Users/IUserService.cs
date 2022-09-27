using InstaRent.Login.Users;

namespace InstaRent.BlazorApp.Services.Users
{
    public interface IUserService
    {
        Task<UserDto?> Login(LoginUserDto user);

        Task<UserDto> GetInfoById(string userId);

        Task<HttpResponseMessage> Create(UserDto user);

        Task<HttpResponseMessage> Update(UserDto user);

    }
}
