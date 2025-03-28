﻿using RDS.Core.Models.ViewModels.Company;

namespace RDS.Web.Services;

public class ManipulateUserStateValuesService(
    UserStateService userState,
    ILocalStorageService localStorage,
    TokenServiceCore tokenServiceCore,
    IApplicationUserHandler userHandler,
    IApplicationUserConfigurationHandler applicationUserConfigurationHandler,
    ICompanyHandler companyHandler,
    AuthenticationStateProvider authenticationStateProvider,
    DeviceService deviceService,
    ISnackbar snackbar)
{
    private List<CompanyIdNameViewModel> CompanyIdsFromUser { get; set; } = [];

    public void SetNotLoggedUserId()
    {
        SetLoggedUserId(0);
    }

    public async Task SetDefaultValuesAsync()
    {
        var loggedUserId = GetLoggedUserId();

        SetSelectedUserName("");
        SetPageTitle("");
        SetCurrentUrl("");
        SetSourceUrl([]);

        SetSelectedUserId(loggedUserId);
        SetSelectedAddressId(0);
        SetSelectedTelephoneId(0);
        SetSelectedCategoryId(0);
        SetSelectedTransactionId(0);

        if (loggedUserId == 0)
        {
            SetUserCompanies([]);
            SetSelectedCompanyId(0);
            return;
        }
        
        var isAdmin = await IsAdminInRolesAsync(loggedUserId);
        var isOwner = isAdmin || await IsOwnerInRolesAsync(loggedUserId);

        SetIsAdmin(isAdmin);
        SetIsOwner(isOwner);

        await SetCompaniesForUser();
        
        // if(isAdmin || isOwner)
        // {
        //     var roleDefault = isAdmin ? "Admin" : "Owner";
        //     var companies = await GetAllCompanyIdNameByRoleAsync(loggedUserId, roleDefault);
        //     if (isAdmin && !companies.Exists(x => x.CompanyId == 9_999_999_999_999))
        //     {
        //         companies.Insert(0,
        //             new CompanyIdNameViewModel
        //                 { CompanyId = 9_999_999_999_999, CompanyName = "Busca em todas as empresas" });
        //     }
        //
        //     SetUserCompanies(companies);
        //     SetSelectedCompanyId(companies.FirstOrDefault()?.CompanyId ?? 0);
        // }
    }

    public async Task SetCompaniesForUser()
    {
        var isAdmin = GetIsAdmin();
        var isOwner = GetIsOwner();
        
        if(isAdmin || isOwner)
        {
            var roleDefault = isAdmin ? "Admin" : "Owner";
            var companies = await GetAllCompanyIdNameByRoleAsync(GetLoggedUserId(), roleDefault);
            if (isAdmin && !companies.Exists(x => x.CompanyId == 9_999_999_999_999))
            {
                companies.Insert(0,
                    new CompanyIdNameViewModel
                        { CompanyId = 9_999_999_999_999, CompanyName = "Busca em todas as empresas" });
            }

            SetUserCompanies(companies);
            SetSelectedCompanyId(companies.FirstOrDefault()?.CompanyId ?? 0);
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

    public async Task<string> GetTokenFromLocalStorageAsync()
    {
        return await localStorage.GetItemAsync<string>("authToken") ?? string.Empty;
    }

    private bool TrySetUserIdFromToken(string token, out long userId, bool validateTokenLifeTime = true)
    {
        userId = 0;
        if (!long.TryParse(tokenServiceCore.GetUserIdFromToken(token, validateTokenLifeTime), out var loggedId))
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
        var result = await userHandler.RefreshTokenAsync(refreshTokenModel);
        if (!result.IsSuccess)
            return false;

        if (showMessage)
            snackbar.Add("Token atualizado com sucesso", Severity.Info);

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
    public bool GetIsAdmin() => userState.GetIsAdmin();
    public bool GetIsOwner() => userState.GetIsOwner();
    public long GetLoggedUserId() => userState.GetLoggedUserId();

    public long GetSelectedUserId()
    {
        return userState.GetSelectedUserId();
        // long selectedUserId = userState.GetSelectedUserId();
        // if (selectedUserId != 0) return selectedUserId;
        //
        // SetSelectedUserId(GetLoggedUserId());
        //
        // return GetSelectedUserId();
    }

    public string GetSelectedUserName() => userState.GetSelectedUserName();
    public long GetSelectedAddressId() => userState.GetSelectedAddressId();
    public long GetSelectedTelephoneId() => userState.GetSelectedTelephoneId();
    public long GetSelectedCompanyId() => userState.GetSelectedCompanyId();
    public List<CompanyIdNameViewModel> GetUserCompanies() => userState.GetUserCompanies();
    public long GetSelectedCategoryId() => userState.GetSelectedCategoryId();
    public long GetSelectedTransactionId() => userState.GetSelectedTransactionId();

    public void SetPageTitle(string title) => userState.SetPageTitle(title);
    public void SetSourceUrl(List<string> urlList) => userState.SetSourceUrl(urlList);
    public void SetCurrentUrl(string url) => userState.SetCurrentUrl(url);
    public void SetIsAdmin(bool isAdmin) => userState.SetIsAdmin(isAdmin);
    public void SetIsOwner(bool isOwner) => userState.SetIsOwner(isOwner);

    public void SetLoggedUserId(long userId)
    {
        if (GetLoggedUserId() == 0 || userId == 0)
            userState.SetLoggedUserId(userId);
    }

    public void SetSelectedUserId(long userId) => userState.SetSelectedUserId(userId);
    public void SetSelectedUserName(string userName) => userState.SetSelectedUserName(userName);
    public void SetSelectedAddressId(long addressId) => userState.SetSelectedAddressId(addressId);
    public void SetSelectedTelephoneId(long telephoneId) => userState.SetSelectedTelephoneId(telephoneId);
    public void SetSelectedCompanyId(long companyId) => userState.SetSelectedCompanyId(companyId);
    public void SetUserCompanies(List<CompanyIdNameViewModel> companies) => userState.SetUserCompanies(companies);
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

    private async Task<List<CompanyIdNameViewModel>> GetAllCompanyIdNameByRoleAsync(long userId, string role)
    {
        try
        {
            var request = new GetAllCompaniesByUserIdRequest
            {
                UserId = userId,
                Role = role
            };
            var result = await companyHandler.GetAllCompanyIdNameByRoleAsync(request);
            if (result.IsSuccess)
                CompanyIdsFromUser = result.Data ?? [];
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
        }

        return CompanyIdsFromUser;
    }

    public async Task<List<ApplicationRole?>> GetRolesAsync()
    {
        var listRoles = new List<ApplicationRole?>();
        try
        {
            var result = await applicationUserConfigurationHandler.ListRoleAsync();
            if (result.IsSuccess)
                listRoles = result.Data ?? [];
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
        }

        return listRoles;
    }

    public async Task<List<ApplicationUserRole?>> GetRolesFromUserAsync(long userId)
    {
        var rolesFromUser = new List<ApplicationUserRole?>();
        try
        {
            var request = new GetAllApplicationUserRoleRequest
            {
                UserId = userId,
                CompanyId = GetSelectedCompanyId()
            };
            var result = await applicationUserConfigurationHandler.ListUserRoleAsync(request);
            if (result.IsSuccess)
                rolesFromUser = result.Data ?? [];
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
        }

        return rolesFromUser;
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

    public async Task<bool> IsOwnerInRolesAsync(long userId)
    {
        var roles = await GetRolesFromUserAsync(userId);

        foreach (var role in roles)
        {
            if (role != null && role.RoleName.Equals("Owner", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> IsHabilitedInRoleAsync(long userId, string roleName)
    {
        var roles = await GetRolesFromUserAsync(userId);

        foreach (var role in roles)
        {
            if (role != null && role.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public async Task<List<string>> GetHabilitedRolesAsync(long userId, List<string> listRoleNames)
    {
        var roles = await GetRolesFromUserAsync(userId);
        var habilitedRoles = new List<string>();

        foreach (var role in roles)
        {
            if (role != null && listRoleNames.Any(rn => rn.Equals(role.RoleName, StringComparison.OrdinalIgnoreCase)))
            {
                habilitedRoles.Add(role.RoleName);
            }
        }

        return habilitedRoles;
    }

    private async Task<bool> IsAdminOrOwnerInRolesAsync(long userId)
    {
        var roles = await GetRolesFromUserAsync(userId);
        return roles.Any(r =>
            r != null &&
            (r.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
             r.RoleName.Equals("Owner", StringComparison.OrdinalIgnoreCase)));
    }

    public async Task<long> GetSelectedUserIdIfAdminAsync()
    {
        var loggedUserId = GetLoggedUserId();
        if (!await IsAdminInRolesAsync(loggedUserId))
        {
            SetSelectedUserId(loggedUserId);
        }

        return GetSelectedUserId();
    }

    public async Task<bool> PermissionOnlyAdminAsync()
    {
        var isAdmin = await StartService.IsAdminInRolesAsync(GetLoggedUserId());
        if (!isAdmin)
        {
            await NavigationService.NavigateToAccessNotAllowedAsync();
        }

        return isAdmin;
    }

    public async Task<bool> PermissionOnlyOwnerAsync()
    {
        var isAdmin = await IsAdminInRolesAsync(GetLoggedUserId());
        var isOwner = await IsOwnerInRolesAsync(GetLoggedUserId());
        if (!isAdmin && !isOwner)
        {
            await NavigationService.NavigateToAccessNotAllowedAsync();
        }

        return isAdmin;
    }

    public async Task<bool> PermissionOnlyAdminOrOwnerAsync()
    {
        var isAllowed = await IsAdminOrOwnerInRolesAsync(GetLoggedUserId());
        if (!isAllowed)
        {
            await NavigationService.NavigateToAccessNotAllowedAsync();
        }

        return isAllowed;
    }
}