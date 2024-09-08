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
    private long LoggedUserId { get; } = StartService.GetLoggedUserId();
    protected long UserId { get; } = StartService.GetSelectedUserId();
    private bool IsAdmin { get; } = StartService.GetIsAdmin();
    private bool IsOwner { get; } = StartService.GetIsOwner();
    protected bool IsNotEdit { get; set; }
    protected const string UserAddressUrl = "/usuarios/enderecos";
    protected const string UserPhoneUrl = "/usuarios/telefones";
    protected string BackUrl = "/";
    protected string CancelOrBackButtonText = "Cancelar";

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

        LoadStartValues();
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

    private void LoadStartValues()
    {
        if (UserId == 0)
        {
            NavigationService.NavigateTo(BackUrl);
        }

        if (!IsAdmin && IsOwner && LoggedUserId != UserId)
        {
            IsNotEdit = true;
            CancelOrBackButtonText = "Voltar";
        }

        if (IsAdmin || IsOwner)
        {
            BackUrl = "/usuarios";
        }
    }

    private async Task LoadUser()
    {
        try
        {
            IsBusy = true;
            var request = new GetApplicationUserByIdRequest { UserId = UserId };
            var response = await UserHandler.GetByIdAsync(request);
            if (response is { IsSuccess: true, Data: not null })
            {
                InputModel = new UpdateApplicationUserRequest
                {
                    UserId = response.Data.Id,
                    Name = response.Data.Name,
                    Email = response.Data.Email!,
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
        {
            await OnValidSubmitAsync();
            NavigationService.NavigateTo(BackUrl);
        }
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