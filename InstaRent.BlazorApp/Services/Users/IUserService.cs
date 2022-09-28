using InstaRent.BlazorApp.Shared.Users;

namespace InstaRent.BlazorApp.Services.Users
{
    public interface IUserService
    {
        event Action OnChange;

        void StateChange();
        Task<UserInfoDto?> Login(UserLoginInfoDto user);

        Task<UserInfoDto> GetInfoById(string userId);

        Task<HttpResponseMessage> Create(UserInfoDto user);

        Task<HttpResponseMessage> Update(UserInfoDto user);

    }
}
