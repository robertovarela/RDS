namespace RDS.Core.Handlers;

public interface IAccountHandler
{
    Task<Response<string>> LoginAsync(LoginRequest request);
    Task<Response<string>> RegisterAsync(CreateApplicationUserRequest request);
    Task LogoutAsync();
    Task<UserInfo?> GetUserInfo();
}