namespace RDS.Web.Pages.ApplicationUsers;

public partial class EditApplicationUsersPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; set; }
    public UpdateApplicationUserRequest InputModel { get; set; } = new();
    protected MudDatePicker Picker = new ();
    public DateTime MinDate = DateTime.Today.AddYears(-110);
    public DateTime MaxDate = DateTime.Today.AddYears(-12);
    protected const string UrlAddress = "/usuarios/enderecos";
    protected const string UrlPhone = "/usuarios/telefones";
    
    #endregion

    #region Services
    [Inject] public IApplicationUserHandler UserHandler { get; set; } = null!;
    [Inject] public LinkUserStateService Link { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var token = await StartService.ValidateAccesByToken();
        var userId = StartService.GetSelectedUserId();
        await LoadUser(userId);
        await StartService.RefreshToken(token, false);
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
    
    private async Task LoadUser(long userId)
    {
        try
        {
            IsBusy = true;
            var request = new GetApplicationUserByIdRequest { UserId = userId };
            var response = await UserHandler.GetByIdAsync(request);
            if (response is { IsSuccess: true, Data: not null })
                InputModel = new UpdateApplicationUserRequest
                {
                    UserId = response.Data.Id,
                    Name = response.Data.Name ?? string.Empty,
                    Email = response.Data.Email ?? string.Empty,
                    Cpf = response.Data.Cpf ?? string.Empty,
                    BirthDate = response.Data.BirthDate
                };
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
    
    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        if (InputModel.Cpf == string.Empty)
        {
            InputModel.Cpf = null;
        }

        try
        {
            var result = await UserHandler.UpdateAsync(InputModel);

            if (result.IsSuccess && result.Data is not null)
            {
                Snackbar.Add("Usuário atualizado", Severity.Success);
            }
            else
            {
                Snackbar.Add($"O usuário {InputModel.UserId}-{InputModel.Name} não foi encontrado", Severity.Warning);
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