namespace RDS.Web.Pages.ApplicationUsers.Telephones;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EditApplicationUserTelephonesPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected UpdateApplicationUserTelephoneRequest InputModel { get; set; } = new();
    private long LoggedUserId { get; } = StartService.GetLoggedUserId();
    private long UserId { get; } = StartService.GetSelectedUserId();
    private bool IsAdmin { get; } = StartService.GetIsAdmin();
    private bool IsOwner { get; } = StartService.GetIsOwner();
    private long TelephoneId { get; set; } = StartService.GetSelectedTelephoneId();
    protected bool IsNotEdit { get; set; }

    protected const string BackUrl = "/usuarios/telefones";
    private const string SourceUrl = "/usuarios/telefones";
    protected string CancelOrBackButtonText = "Cancelar";

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

        LoadStartValues();
        await LoadUserAddressAsync();
    }

    #endregion

    #region Methods

    private void LoadStartValues()
    {
        if (UserId == 0 || TelephoneId == 0)
        {
            NavigationService.NavigateTo(SourceUrl);
        }
        
        if (!IsAdmin && IsOwner && LoggedUserId != UserId)
        {
            IsNotEdit = true;
            CancelOrBackButtonText = "Voltar";
        }
    }

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