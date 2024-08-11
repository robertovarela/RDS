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

    private void HandleInvalidToken()
    {
        NavigationService.NavigateToLogin();
    }

    public async Task<string> ValidateAccessByTokenOld()
    {
        string token = await localStorage.GetItemAsync<string>("authToken") ?? string.Empty;
        if (string.IsNullOrEmpty(token)) return token;

        if (long.TryParse(tokenService.GetUserIdFromToken(token), out var id))
        {
            userState.SetLoggedUserId(id);
            if (id != 0) return token;
        }

        if (!long.TryParse(tokenService.GetUserIdFromToken(token, false), out var newId)) return token;
        if (newId == 0)
        {
            snackbar.Add("Token inválido!", Severity.Warning);
            NavigationService.NavigateToLogin();
            return token;
        }

        userState.SetLoggedUserId(newId);
        await RefreshToken(token, true);

        return token;
    }

    public async Task<bool> RefreshToken(string token, bool showMessage)
    {
        var fingerprint = await deviceService.GetDeviceFingerprint();
        var refreshTokenModel = new RefreshTokenRequest { Token = token, FingerPrint = fingerprint };
        var result = await authenticationService.RefreshTokenAsync(refreshTokenModel);

        if (!result) return false;
        if (showMessage) snackbar.Add("Token atualizado com sucesso", Severity.Info);

        return true;
    }

    public long GetLoggedUserId()
    {
        long loggedUserId = userState.GetLoggedUserId();
        return loggedUserId;
    }

    public long GetSelectedUserId()
    {
        long selectedUserId = userState.GetSelectedUserId();
        if (selectedUserId != 0) return selectedUserId;
        userState.SetSelectedUserId(userState.GetLoggedUserId());
        selectedUserId = userState.GetSelectedUserId();
        return selectedUserId;
    }

    public long GetSelectedAddressId()
    {
        long selectedAddressId = userState.GetSelectedAddressId();

        return selectedAddressId;
    }

    public long GetSelectedCategoryId()
    {
        long selectedCategoryId = userState.GetSelectedCategoryId();

        return selectedCategoryId;
    }

    public void SetSelectedUserId(long userId)
    {
        userState.SetSelectedUserId(userId);
    }

    public void SetSelectedAddressId(long addressId)
    {
        userState.SetSelectedAddressId(addressId);
    }

    public void SetSelectedCategoryId(long categoryId)
    {
        userState.SetSelectedCategoryId(categoryId);
    }
}