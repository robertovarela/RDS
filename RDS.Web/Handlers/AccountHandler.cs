namespace RDS.Web.Handlers;

public class AccountHandler(HttpClient httpClient) : IAccountHandler
{
    public async Task<Response<string>> LoginAsync(LoginRequest request)
    {
        var result = await httpClient.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
        return result.IsSuccessStatusCode
            ? new Response<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!")
            : new Response<string>(null, 400, "Não foi possível realizar o login");
    }

    public async Task<Response<string>> RegisterAsync(CreateApplicationUserRequest request)
    {
        var result = await httpClient.PostAsJsonAsync("v1/useridentity/createuser", request);
        return result.IsSuccessStatusCode
            ? new Response<string>("Cadastro realizado com sucesso!", 201, "Cadastro realizado com sucesso!")
            : new Response<string>(null, 400, "Não foi possível realizar o seu cadastro");
    }

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        await httpClient.PostAsJsonAsync("v1/useridentity/logout", emptyContent);
    }

    public async Task<UserInfo?> GetUserInfo()
    {
        return await httpClient.GetFromJsonAsync<UserInfo>("v1/useridentity/manager/userinfo");
    }
}