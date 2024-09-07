namespace RDS.Web.Pages.ApplicationUsers.Addresses;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ListApplicationUserAdressesPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected List<ApplicationUserAddress> ApplicationUsersAddress { get; private set; } = [];
    private long UserId { get; set; }
    protected string SearchTerm { get; set; } = string.Empty;
    protected const string BackUrl = "/usuarios/editar";
    protected const string AddUrl = "/usuarios/enderecos/adicionar";
    protected const string EditUrl = "/usuarios/enderecos/editar";
    protected const string OrigenUrl = "/usuarios/enderecos";

    #endregion

    #region Services

    [Inject] protected IApplicationUserAddressHandler AddressHandler { get; set; } = null!;
    [Inject] protected ISnackbar Snackbar { get; set; } = null!;
    [Inject] protected IDialogService DialogService { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Endereços");
        await StartService.ValidateAccesByTokenAsync();
        UserId = StartService.GetSelectedUserId();

        await LoadAddressesAsync();
    }

    #endregion

    #region Methods

    private async Task LoadAddressesAsync()
    {
        IsBusy = true; 

        try
        {
            var request = new GetAllApplicationUserAddressRequest{CompanyId = UserId};
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
    
    protected async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Escape" || e.CtrlKey)
        {
            SearchTerm = string.Empty;
            StateHasChanged();
        }
        await JsRuntime.InvokeVoidAsync("triggerEnterOnElement", "myInputFieldId");
    }
    
    protected async void OnDeleteButtonClickedAsync(long userId, long id, string street)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir o endereço ( {id} - {street} ) será excluído. Esta é uma ação irreversível! Deseja continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(userId, id);

        StateHasChanged();
    }

    private async Task OnDeleteAsync(long userId, long id)
    {
        try
        {
            var request = new DeleteApplicationUserAddressRequest 
            { 
                CompanyId = userId,  
                Id = id
            };
            var result = await AddressHandler.DeleteAsync(request);
            ApplicationUsersAddress.RemoveAll(x => x.UserId == userId && x.Id == id);
            Snackbar.Add(result.Message!, result.Data != null ? Severity.Success : Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    protected Func<ApplicationUserAddress, bool> Filter => applicationUserAddress =>
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