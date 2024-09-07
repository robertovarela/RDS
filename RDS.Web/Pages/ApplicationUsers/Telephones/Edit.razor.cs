namespace RDS.Web.Pages.ApplicationUsers.Telephones;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EditApplicationUserTelephonesPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected UpdateApplicationUserTelephoneRequest InputModel { get; set; } = new();
    private long LoggedUserId { get; set; }
    private long UserId { get; set; }
    private bool IsAdmin { get; set; }
    private bool IsOwner { get; set; }
    private long TelephoneId { get; set; }
    protected bool IsNotEdit { get; set; }
    private const string BackUrl = "/usuarios/telefones";
    protected const string CancelUrl = "/usuarios/telefones";
    private const string OrigenUrl = "/usuarios/telefones";

    #endregion

    #region Parameters

    [Parameter] public IMask BrazilPhoneNumberMask { get; set; } = new PatternMask("(00) 00000-0000");

    #endregion

    #region Services

    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IApplicationUserTelephoneHandler TelephoneHandler { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Editar Telefone");
        await StartService.ValidateAccesByTokenAsync();
        LoggedUserId = StartService.GetLoggedUserId();
        UserId = StartService.GetSelectedUserId();
        TelephoneId = StartService.GetSelectedTelephoneId();
        if (UserId == 0)
        {
            NavigationService.NavigateTo(OrigenUrl);
        }
        IsAdmin = await StartService.IsAdminInRolesAsync(LoggedUserId);
        if (!IsAdmin)
        {
            IsOwner = await StartService.IsOwnerInRolesAsync(LoggedUserId);
            IsNotEdit = IsOwner && (LoggedUserId != UserId);
        }

        await LoadUserAddressAsync();
    }

    #endregion

    #region Methods

    private async Task LoadUserAddressAsync()
    {
        IsBusy = true;
        try
        {
            var requestTelephone = new GetApplicationUserTelephoneByIdRequest()
            {
                Id = TelephoneId
            };
            var responseTelephone = await TelephoneHandler.GetByIdAsync(requestTelephone);
            if (responseTelephone is { IsSuccess: true, Data: not null })
            {
                InputModel = new UpdateApplicationUserTelephoneRequest
                {
                    Id = responseTelephone.Data.Id,
                    Number = responseTelephone.Data.Number,
                    Type = responseTelephone.Data.Type,
                    UserId = responseTelephone.Data.UserId
                };
            }
            else
            {
                Snackbar.Add("Telefone não encontrado", Severity.Warning);
                NavigationService.NavigateTo(BackUrl);
            }
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
    
    protected async void OnUpdateButtonClickedAsync()
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir o telefone será atualizado. Deseja continuar?",
            yesText: "ALTERAR",
            cancelText: "Cancelar");

        if (result is true)
            await OnValidSubmitAsync();

        StateHasChanged();
    }
    
    private async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await TelephoneHandler.UpdateAsync(InputModel);

            if (result.IsSuccess && result.Data is not null)
            {
                Snackbar.Add("Telefone atualizado", Severity.Success);
            }
            else
            {
                Snackbar.Add($"O telefone {InputModel.Number} não foi encontrado", Severity.Warning);

            }

            NavigationService.NavigateTo(BackUrl);
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
}