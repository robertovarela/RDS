using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RDS.Core.Handlers;
using RDS.Core.Models.ApplicationUser;
using RDS.Core.Requests.ApplicationUsers;
using RDS.Core.Services;
using RDS.Web.Services;

namespace RDS.Web.Pages.ApplicationUsers;

public class ListApplicationUsersPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<ApplicationUser> ApplicationUsers { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;
    public string Url { get; set; } = "/usuarios/editar";

    #endregion

    #region Services

    [Inject] private TokenService TokenService { get; set; } = null!;
    [Inject] private HttpClientService HttpClientService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;
    //[Inject] private ManipulateUserStateValuesService ManipulateUserStateValues { get; set; } = null!;
    [Inject] public UserStateService UserState { get; set; } = null!;
    [Inject] public IApplicationUserHandler UserHandler { get; set; } = null!;
    [Inject] public IAccountHandler AccountHandler { get; set; } = null!;
    [Inject] public LinkUserStateService Link { get; set; } = null!;

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        //var userId = await ManipulateUserStateValues.SetDefaultValues();
        var userId = await StartService.SetDefaultValues();
        
        var userIdTemp = await StartService.GetSelectedUserId();

        IsBusy = true;
        try
        {
            var request = new GetAllApplicationUserRequest {UserId = userId};
            var result = await UserHandler.GetAllAsync(request);
            if (result.IsSuccess)
                ApplicationUsers = result.Data ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Methods

    public async void OnDeleteButtonClickedAsync(long id, string name)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir o usuário ( {id} - {name} ) será excluído. Esta é uma ação irreversível! Deseja continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id);

        StateHasChanged();
    }

    public async Task OnDeleteAsync(long id)
    {
        try
        {
            var request = new DeleteApplicationUserRequest { UserId = id };
            var result = await UserHandler.DeleteAsync(request);
            ApplicationUsers.RemoveAll(x => x.Id == id);
            if (result.Data != null)
            {
                Snackbar.Add(result.Message, Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Warning);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    public Func<ApplicationUser, bool> Filter => applicationUser =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;

        if (applicationUser.Id.ToString().Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (applicationUser.Name is not null &&
            applicationUser.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (applicationUser.Email is not null &&
            applicationUser.Email.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (applicationUser.Cpf is not null &&
            applicationUser.Cpf.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;
        
        return false;
    };

    public async void OnNewUserButton()
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir será efetuado logout para permitir o cadastro de um novo usuário. Deseja continuar?",
            yesText: "LOGOUT",
            cancelText: "Cancelar");

        if (result is true)
            OnNewUser();
    }

    private void OnNewUser()
    {
        NavigationService.NavigateToRegister();
    }

    public async Task<long> GetUserSeletctedTemp()
    {
        return await StartService.GetSelectedUserId();
    }
    
    #endregion
}