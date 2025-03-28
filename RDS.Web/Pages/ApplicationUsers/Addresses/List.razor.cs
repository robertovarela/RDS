namespace RDS.Web.Pages.ApplicationUsers.Addresses;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ListApplicationUserAddressesPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected List<ApplicationUserAddress> ApplicationUsersAddress { get; private set; } = [];
    private long LoggedUserId { get; } = StartService.GetLoggedUserId();
    private long UserId { get; } = StartService.GetSelectedUserId();
    private bool IsAdmin { get; } = StartService.GetIsAdmin();
    private bool IsOwner { get; } = StartService.GetIsOwner();
    protected bool IsNotEdit { get; set; }
    protected string SearchTerm { get; set; } = string.Empty;
    protected const string BackUrl = "/usuarios/editar";
    protected const string AddUrl = "/usuarios/enderecos/adicionar";
    protected const string EditUrl = "/usuarios/enderecos/editar";

    #endregion

    #region Services

    [Inject] private IApplicationUserAddressHandler AddressHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Endereços");
        await StartService.ValidateAccesByTokenAsync();

        LoadStartValues();
        await LoadAddressesAsync();
    }

    #endregion

    #region Methods

    private void LoadStartValues()
    {
        if (UserId == 0)
        {
            NavigationService.NavigateTo(BackUrl);
        }
        if (!IsAdmin)
        {
            IsNotEdit = IsOwner && (LoggedUserId != UserId);
        }
    }

    private async Task LoadAddressesAsync()
    {
        IsBusy = true; 

        try
        {
            var request = new GetAllApplicationUserAddressRequest{UserId = UserId};
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
    
    protected void HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key != "Escape" && !e.CtrlKey) return;
        SearchTerm = string.Empty;
        StateHasChanged();
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
            Snackbar.Add(result.Message, result.Data != null ? Severity.Success : Severity.Warning);
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