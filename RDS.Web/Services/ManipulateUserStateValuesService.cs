namespace RDS.Web.Services;

public class ManipulateUserStateValuesService(
    UserStateService userState,
    ILocalStorageService localStorage,
    TokenService tokenService,
    AuthenticationService authenticationService,
    DeviceService deviceService,
    ISnackbar snackbar)
{
    public void SetDefaultValues()
    {
        userState.SetSelectedUserId(userState.GetLoggedUserId());
        userState.SetSelectedAddressId(0);
        userState.SetSelectedCategoryId(0);
    }

    public async Task ValidateAccessByToken()
    {
        string token = await GetTokenFromLocalStorage();
        if (string.IsNullOrEmpty(token))
        {
            snackbar.Add("Token não encontrado", Severity.Warning);
            HandleInvalidToken();
            return;
        }

        if (TrySetUserIdFromToken(token, out var id) && id != 0) return;

        if (ConfigurationWeb.RenewToken)
        {
            if (!TrySetUserIdFromToken(token, out var newId, validateToken: false) || newId == 0)
            {
                snackbar.Add("Token inválido!", Severity.Warning);
                HandleInvalidToken();
                return;
            }

            if (await RefreshToken(token, showMessage: ConfigurationWeb.RenewTokenMessage)) return;

            snackbar.Add("Não foi possível atualizar o token", Severity.Error);
        }

        HandleInvalidToken();
    }

    private async Task<string> GetTokenFromLocalStorage()
    {
        return await localStorage.GetItemAsync<string>("authToken") ?? string.Empty;
    }

    private bool TrySetUserIdFromToken(string token, out long userId, bool validateToken = true)
    {
        userId = 0;
        if (!long.TryParse(tokenService.GetUserIdFromToken(token, validateToken), out var id))
            return false;

        userState.SetLoggedUserId(id);
        userId = id;
        return true;
    }

    private static void HandleInvalidToken() => NavigationService.NavigateToLogin();

    public async Task<bool> RefreshToken(string token, bool showMessage)
    {
        var fingerprint = await deviceService.GetDeviceFingerprint();
        var refreshTokenModel = new RefreshTokenRequest { Token = token, FingerPrint = fingerprint };
        var result = await authenticationService.RefreshTokenAsync(refreshTokenModel);

        if (!result) return false;
        if (showMessage) snackbar.Add("Token atualizado com sucesso", Severity.Info);

        return true;
    }

    public bool ValidateSourceUrl(List<string> sourceUrl,
        string currentUrl, 
        bool navigateToAccessNotAllowed = true, 
        bool showMessage = true)
    {
        var commonUrls = sourceUrl.Intersect(GetSourceUrl()).ToList();

        if (commonUrls.Any()) return true;
        
        if (GetCurrentUrl().Equals(currentUrl)) return true;
        
        if(showMessage)
        {
            snackbar.Add("Acesso não permitido!", Severity.Error);
            snackbar.Add("URL de origem não reconhecida", Severity.Error);
        }
        
        if(navigateToAccessNotAllowed)
        {
            NavigationService.NavigateToAccessNotAllowed();
            return false;
        }
        
        NavigationService.NavigateToLogin();
        
        return false;
    }
    
    public string GetPageTitle() => userState.GetPageTitle();
    public List<string> GetSourceUrl() => userState.GetSourceUrl();
    public string GetCurrentUrl() => userState.GetCurrentUrl();
    public long GetLoggedUserId() =>userState.GetLoggedUserId();

    public long GetSelectedUserId()
    {
        long selectedUserId = userState.GetSelectedUserId();
        if (selectedUserId == 0)
        {
            userState.SetSelectedUserId(userState.GetLoggedUserId());
            selectedUserId = userState.GetSelectedUserId();
        }

        return selectedUserId;
    }
    public string GetSelectedUserName() => userState.GetSelectedUserName();
    public long GetSelectedAddressId() => userState.GetSelectedAddressId();
    public long GetSelectedCategoryId() => userState.GetSelectedCategoryId();

    public void SetPageTitle(string title) => userState.SetPageTitle(title);
    public void SetSourceUrl(List<string> urlList) => userState.SetSourceUrl(urlList);
    public void SetCurrentUrl(string url) => userState.SetCurrentUrl(url);
    public void SetSelectedUserId(long userId) => userState.SetSelectedUserId(userId);
    public void SetSelectedUserName(string userName) => userState.SetSelectedUserName(userName);
    public void SetSelectedAddressId(long addressId) => userState.SetSelectedAddressId(addressId);
    public void SetSelectedCategoryId(long categoryId) => userState.SetSelectedCategoryId(categoryId);
}