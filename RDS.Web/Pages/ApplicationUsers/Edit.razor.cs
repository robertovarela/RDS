namespace RDS.Web.Pages.ApplicationUsers;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EditApplicationUsersPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; set; }
    protected UpdateApplicationUserRequest InputModel { get; set; } = new();
    protected MudDatePicker Picker = new();
    protected DateTime MinDate = DateTime.Today.AddYears(-110);
    protected DateTime MaxDate = DateTime.Today.AddYears(-12);
    private long UserId { get; set; }
    private bool IsAdmin { get; set; }
    private bool IsOwner { get; set; }
    private string Email { get; set; } = null!;
    protected bool IsNotEdit { get; set; }
    protected const string UrlAddress = "/usuarios/enderecos";
    protected const string UrlPhone = "/usuarios/telefones";
    protected const string UrlOrigen = "/usuarios";

    #endregion

    #region Services

    [Inject] protected IApplicationUserHandler UserHandler { get; set; } = null!;
    [Inject] protected ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Editar Usuário");
        await StartService.ValidateAccesByTokenAsync();
        //UserId = await StartService.GetSelectedUserIdIfAdminAsync();
        UserId = StartService.GetSelectedUserId();
        IsAdmin = await StartService.IsAdminInRolesAsync(StartService.GetLoggedUserId());
        if (!IsAdmin)
        {
            IsOwner = await StartService.IsOwnerInRolesAsync(UserId);
            IsNotEdit = IsOwner && (StartService.GetLoggedUserId() != UserId);
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

    protected async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        if (InputModel.Cpf == string.Empty)
        {
            InputModel.Cpf = null;
        }

        try
        {
            InputModel.Email = Email;
            var result = await UserHandler.UpdateAsync(InputModel);

            if (result.IsSuccess && result.Data is not null)
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