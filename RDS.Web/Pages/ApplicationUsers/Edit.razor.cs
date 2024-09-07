namespace RDS.Web.Pages.ApplicationUsers;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EditApplicationUsersPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected UpdateApplicationUserRequest InputModel { get; set; } = new();
    protected MudDatePicker Picker = new();
    protected DateTime MinDate = DateTime.Today.AddYears(-110);
    protected DateTime MaxDate = DateTime.Today.AddYears(-12);
    private long LoggedUserId { get; set; }
    protected long UserId { get; private set; }
    private bool IsAdmin { get; set; }
    private bool IsOwner { get; set; }
    private string Email { get; set; } = null!;
    protected bool IsNotEdit { get; set; }
    protected const string UserAddressUrl = "/usuarios/enderecos";
    protected const string UserPhoneUrl = "/usuarios/telefones";
    protected string CancelUrl = "/";
    private const string OrigenUrl = "/usuarios";

    #endregion

    #region Services

    [Inject] private IApplicationUserHandler UserHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Editar Usuário");
        await StartService.ValidateAccesByTokenAsync();
        LoggedUserId = StartService.GetLoggedUserId();
        UserId = StartService.GetSelectedUserId();
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

        if (IsAdmin || IsOwner)
        {
            CancelUrl = "/usuarios";
        }
        
        await LoadUser();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (string.IsNullOrEmpty(InputModel.BirthDate.ToString()))
        {
            Picker.GoToDate(MaxDate, false);
        }
    }

    #endregion

    #region Methods

    private async Task LoadUser()
    {
        try
        {
            IsBusy = true;
            var request = new GetApplicationUserByIdRequest { UserId = UserId };
            var response = await UserHandler.GetByIdAsync(request);
            if (response is { IsSuccess: true, Data: not null })
            {
                Email = response.Data.Email!;
                InputModel = new UpdateApplicationUserRequest
                {
                    UserId = response.Data.Id,
                    Name = response.Data.Name,
                    Email = Email,
                    Cpf = response.Data.Cpf ?? string.Empty,
                    BirthDate = response.Data.BirthDate
                };
            }
        }
        catch
        {
            Snackbar.Add("Não foi possível obter os dados do usuário", Severity.Error);
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
            $"Ao prosseguir o cadastro será atualizado. Deseja continuar?",
            yesText: "ALTERAR",
            cancelText: "Cancelar");

        if (result is true)
            await OnValidSubmitAsync();

        StateHasChanged();
    }
    private async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        if (string.IsNullOrWhiteSpace(InputModel.Cpf))
        {
            InputModel.Cpf = null;
        }
        
        try
        {
            var result = await UserHandler.UpdateAsync(InputModel);

            if (result is { IsSuccess: true, Data: not null })
            {
                Snackbar.Add("Usuário atualizado", Severity.Success);
            }
            else
            {
                Snackbar.Add($"O usuário {InputModel.CompanyId}-{InputModel.Name} não foi encontrado",
                    Severity.Warning);
            }

            StateHasChanged();
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