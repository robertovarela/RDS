namespace RDS.Web.Pages.ApplicationUsers.Telephones;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ListApplicationUserTelephonesPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected List<ApplicationUserTelephone> ApplicationUsersTelephone { get; private set; } = [];
    private long LoggedUserId { get; } = StartService.GetLoggedUserId();
    private long UserId { get; } = StartService.GetSelectedUserId();
    private bool IsAdmin { get; } = StartService.GetIsAdmin();
    private bool IsOwner { get; } = StartService.GetIsOwner();
    protected bool IsNotEdit { get; set; }
    protected string SearchTerm { get; set; } = string.Empty;
    protected const string AddUrl = "/usuarios/telefones/adicionar";
    protected const string EditUrl = "/usuarios/telefones/editar";
    protected const string BackUrl = "/usuarios/editar";

    #endregion

    #region Services

    [Inject] private IApplicationUserTelephoneHandler TelephoneHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Telefones");
        await StartService.ValidateAccesByTokenAsync();

        LoadStartValues();
        await LoadAddressesAsync();
    }

    #endregion

    #region Methods

    private void LoadStartValues()
    {
        if (!IsAdmin && IsOwner && LoggedUserId != UserId)
        {
            IsNotEdit = true;
        }
    }
    private async Task LoadAddressesAsync()
    {
        IsBusy = true; 

        try
        {
            var request = new GetAllApplicationUserTelephoneRequest(){UserId = UserId};
            var result = await TelephoneHandler.GetAllAsync(request);
            if (result.IsSuccess)
                ApplicationUsersTelephone = result.Data ?? [];
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
    
    protected async void OnDeleteButtonClickedAsync(long userId, long id, string telephone)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir o endereço ( {id} - {telephone} ) será excluído. Esta é uma ação irreversível! Deseja continuar?",
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
            var request = new DeleteApplicationUserTelephoneRequest()
            { 
                UserId = userId,  
                Id = id
            };
            var result = await TelephoneHandler.DeleteAsync(request);
            ApplicationUsersTelephone.RemoveAll(x => x.UserId == userId && x.Id == id);
            Snackbar.Add(result.Message!, result.Data != null ? Severity.Success : Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    protected Func<ApplicationUserTelephone, bool> Filter => applicationUserTelephone 
        => string.IsNullOrWhiteSpace(SearchTerm) 
           || applicationUserTelephone.Number.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);

    #endregion
}