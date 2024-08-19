namespace RDS.Web.Pages.ApplicationUsers.Addresses;

public class ListApplicationUserAdressesPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected List<ApplicationUserAddress> ApplicationUsersAddress { get; private set; } = [];
    protected string SearchTerm { get; set; } = string.Empty;
    protected const string Url = "/usuarios/enderecos/editar";
    protected const string UrlOrigen = "/usuarios/enderecos";

    #endregion

    #region Services

    [Inject] public IApplicationUserAddressHandler AddressHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Endereços");
        await StartService.ValidateAccesByToken();
        var userId = StartService.GetSelectedUserId();
        IsBusy = true; 

        try
        {
            var request = new GetAllApplicationUserAddressRequest{UserId = userId};
            var result = await AddressHandler.GetAllAsync(request);
            if (result.IsSuccess)
                ApplicationUsersAddress = result.Data ?? [];
        }
        catch (Exception)
        {
            Snackbar.Add("Não foi possível obter a lista de endereços", Severity.Error);
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

        if (applicationUserAddress.PostalCode.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (applicationUserAddress.Street.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    #endregion
}