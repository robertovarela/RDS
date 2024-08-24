﻿namespace RDS.Web.Services;

public class ManipulateUserStateValuesService(
    UserStateService userState,
    ILocalStorageService localStorage,
    TokenService tokenService,
    AuthenticationService authenticationService,
    IApplicationUserConfigurationHandler applicationUserConfigurationHandler,
    ICompanyHandler companyHandler,
    AuthenticationStateProvider authenticationStateProvider,
    DeviceService deviceService,
    ISnackbar snackbar)
{
    private List<ApplicationUserRole?> RolesFromUser { get; set; } = [];
    private List<AllCompaniesIdViewModel> CompanyIdsFromUser { get; set; } = [];

    public async Task SetDefaultValuesAsync()
    {
        userState.SetSelectedUserId(userState.GetLoggedUserId());
        userState.SetSelectedAddressId(0);
        userState.SetSelectedCategoryId(0);
        userState.SetSelectedTransactionId(0);

        long loggedUserId = GetLoggedUserId();
        if (await IsAdminInRolesAsync(loggedUserId))
        {
            var companies = await GetCompaniesByUserIdAsync(loggedUserId);

            if (!companies.Any()) SetSelectedCompanyId(0);

            SetSelectedCompanyId(companies.First().CompanyId);
        }
    }

    public async Task ValidateAccessByTokenAsync(bool blockNavigation = true)
    {
        string token = await GetTokenFromLocalStorageAsync();
        if (string.IsNullOrEmpty(token))
        {
            snackbar.Add("Token não encontrado", Severity.Warning);
            HandleInvalidToken(blockNavigation);
            return;
        }

        if (TrySetUserIdFromToken(token, out var id) && id != 0) return;

        if (ConfigurationWeb.RenewToken)
        {
            if (!TrySetUserIdFromToken(token, out var newId, validateTokenLifeTime: false) || newId == 0)
            {
                snackbar.Add("Token inválido!", Severity.Warning);
                HandleInvalidToken(blockNavigation);
                return;
            }

            if (await RefreshToken(token, showMessage: ConfigurationWeb.RenewTokenMessage)) return;

            snackbar.Add("Não foi possível atualizar o token", Severity.Error);
        }

        HandleInvalidToken(blockNavigation);
    }

    private async Task<string> GetTokenFromLocalStorageAsync()
    {
        return await localStorage.GetItemAsync<string>("authToken") ?? string.Empty;
    }

    private bool TrySetUserIdFromToken(string token, out long userId, bool validateTokenLifeTime = true)
    {
        userId = 0;
        if (!long.TryParse(tokenService.GetUserIdFromToken(token, validateTokenLifeTime), out var loggedId))
            return false;

        SetLoggedUserId(loggedId);
        userId = loggedId;
        return true;
    }

    private static void HandleInvalidToken(bool blockNavigation)
    {
        if (blockNavigation) NavigationService.NavigateToLogin();
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

    public async Task<bool> ValidateSourceUrl(List<string> sourceUrl,
        string currentUrl,
        bool navigateToAccessNotAllowed = true,
        bool showMessage = true)
    {
        var commonUrls = sourceUrl.Intersect(GetSourceUrl()).ToList();

        if (commonUrls.Any()) return true;
        //var currentUrlMemory = GetSourceUrl();
        if (GetCurrentUrl().Equals(currentUrl)) return true;

        if (showMessage)
        {
            snackbar.Add("Acesso não permitido!", Severity.Error);
            snackbar.Add("URL de origem não reconhecida", Severity.Error);
        }

        if (navigateToAccessNotAllowed)
        {
            await NavigationService.NavigateToAccessNotAllowedAsync();
            return false;
        }

        NavigationService.NavigateToLogin();

        return false;
    }

    public string GetPageTitle() => userState.GetPageTitle();
    public List<string> GetSourceUrl() => userState.GetSourceUrl();
    public string GetCurrentUrl() => userState.GetCurrentUrl();
    public long GetLoggedUserId() => userState.GetLoggedUserId();

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
    public long GetSelectedCompanyId() => userState.GetSelectedCompanyId();
    public long GetSelectedCategoryId() => userState.GetSelectedCategoryId();
    public long GetSelectedTransactionId() => userState.GetSelectedTransactionId();

    public void SetPageTitle(string title) => userState.SetPageTitle(title);
    public void SetSourceUrl(List<string> urlList) => userState.SetSourceUrl(urlList);
    public void SetCurrentUrl(string url) => userState.SetCurrentUrl(url);
    private void SetLoggedUserId(long userId) => userState.SetLoggedUserId(userId);
    public void SetSelectedUserId(long userId) => userState.SetSelectedUserId(userId);
    public void SetSelectedUserName(string userName) => userState.SetSelectedUserName(userName);
    public void SetSelectedAddressId(long addressId) => userState.SetSelectedAddressId(addressId);
    public void SetSelectedCompanyId(long companyId) => userState.SetSelectedCompanyId(companyId);
    public void SetSelectedCategoryId(long categoryId) => userState.SetSelectedCategoryId(categoryId);
    public void SetSelectedTransactionId(long transactionId) => userState.SetSelectedTransactionId(transactionId);

    public async Task VerifyIfLoggedInAsync(string destinationUrlLoggedIn = "/", string destinationUrlNotLoggedIn = "")
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            if (!destinationUrlLoggedIn.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                NavigationService.NavigateTo(destinationUrlLoggedIn);
            }

            return;
        }

        if (!destinationUrlNotLoggedIn.Equals("", StringComparison.OrdinalIgnoreCase))
        {
            NavigationService.NavigateTo(destinationUrlNotLoggedIn);
        }
    }

    public async Task<List<AllCompaniesIdViewModel>> GetCompaniesByUserIdAsync(long userId)
    {
        try
        {
            var request = new GetAllCompaniesByUserIdRequest { UserId = userId };
            var result = await companyHandler.GetAllCompanyIdByUserIdAsync(request);
            if (result.IsSuccess)
                CompanyIdsFromUser = result.Data ?? [];
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
        }

        return CompanyIdsFromUser;
    }

    public async Task<List<ApplicationUserRole?>> GetRolesFromUserAsync(long companyId)
    {
        try
        {
            var request = new GetAllApplicationUserRoleRequest { CompanyId = companyId };
            var result = await applicationUserConfigurationHandler.ListUserRoleAsync(request);
            if (result.IsSuccess)
                RolesFromUser = result.Data ?? [];
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
        }

        return RolesFromUser;
    }

    public async Task<bool> IsAdminInRolesAsync(long userId)
    {
        var roles = await GetRolesFromUserAsync(userId);

        foreach (var role in roles)
        {
            if (role != null && role.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public async Task<long> GetSelectedUserIdIfAdminAsync()
    {
        var loggedUserId = GetLoggedUserId();
        var isAdmin = await IsAdminInRolesAsync(loggedUserId);
        return isAdmin ? GetSelectedUserId() : loggedUserId;
    }
}