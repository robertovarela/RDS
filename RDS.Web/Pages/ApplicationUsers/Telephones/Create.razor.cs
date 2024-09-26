namespace RDS.Web.Pages.ApplicationUsers.Telephones;

// ReSharper disable once PartialTypeWithSinglePart
public partial class CreateApplicationUserTelephonePage : ComponentBase
{
    #region Properties

    public CreateApplicationUserTelephoneRequest InputModel { get; set; } = new();
    private long UserId { get; } = StartService.GetSelectedUserId();
    protected bool IsNotEdit { get; set; }
    
    protected const string BackUrl = "/usuarios/telefones";

    #endregion

    #region Parameters

    [Parameter] public IMask BrazilPhoneNumberMask { get; set; } = new PatternMask("(00) 00000-0000");

    #endregion

    #region Services

    [Inject] private IApplicationUserTelephoneHandler TelephoneHandler { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides
    
    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Novo Telefone");
        await StartService.ValidateAccesByTokenAsync();

        LoadStartValues();
    }
    
    #endregion
    
    #region Methods

    private void LoadStartValues()
    {
        if (UserId == 0)
        {
            NavigationService.NavigateTo(BackUrl);
        }

        if (UserId != StartService.GetLoggedUserId())
        {
            IsNotEdit = true;
        }

        InputModel.Type = ETypeOfPhone.Celular;
    }
    
    public async Task OnValidSubmitAsync()
    {
        try
        {
            InputModel.UserId = StartService.GetSelectedUserId();
            var result = await TelephoneHandler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationService.NavigateTo("/usuarios/telefones");
            }
            else
                Snackbar.Add(result.Message, Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    #endregion
}