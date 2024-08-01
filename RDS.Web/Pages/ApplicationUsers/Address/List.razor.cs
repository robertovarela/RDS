using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RDS.Core.Handlers;
using RDS.Core.Models.ApplicationUser;
using RDS.Core.Requests.ApplicationUsers.Address;
using RDS.Core.Services;
using RDS.Web.Services;

namespace RDS.Web.Pages.ApplicationUsers;

public partial class ListApplicationUserAdressesPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<ApplicationUserAddress> ApplicationUsersAddress { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;

    public string Url { get; set; } = "/usuarios/enderecos/editar";

    #endregion

    #region Services

    [Inject] private TokenService TokenService { get; set; } = null!;
    [Inject] private HttpClientService HttpClientService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;
    [Inject] private ManipulateUserStateValuesService ManipulateUserStateValues { get; set; } = null!;
    [Inject] public UserStateService UserState { get; set; } = null!;
    [Inject] public IApplicationUserHandler UserHandler { get; set; } = null!;
    [Inject] public IApplicationUserAddressHandler AddressHandler { get; set; } = null!;
    [Inject] public LinkUserStateService Link { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var userId = await ManipulateUserStateValues.GetSelectedUserId();
        userId = await StartService.GetSelectedUserId();
        IsBusy = true; 

        try
        {
            var request = new GetAllApplicationUserAddressRequest{UserId = userId};
            var result = await AddressHandler.GetAllAsync(request);
            if (result.IsSuccess)
                ApplicationUsersAddress = result.Data ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
            StateHasChanged();
        }
    }

    #endregion

    #region Methods

    public async void OnDeleteButtonClickedAsync(long userId, long id, string street)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir o endereço ( {id} - {street} ) será excluído. Esta é uma ação irreversível! Deseja continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(userId, id, street);

        StateHasChanged();
    }

    public async Task OnDeleteAsync(long userId, long id, string title)
    {
        try
        {
            var request = new DeleteApplicationUserAddressRequest 
            { 
                UserId = userId,  
                Id = id
            };
            var result = await AddressHandler.DeleteAsync(request);
            ApplicationUsersAddress.RemoveAll(x => x.UserId == userId && x.Id == id);
            if(result.Data != null)
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

    public Func<ApplicationUserAddress, bool> Filter => applicationUserAddress =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;

        if (applicationUserAddress.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (applicationUserAddress.PostalCode is not null &&
            applicationUserAddress.PostalCode.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (applicationUserAddress.Street is not null &&
            applicationUserAddress.Street.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    #endregion
}