namespace RDS.Web.Pages.ApplicationUsers.Telephones;

// ReSharper disable once PartialTypeWithSinglePart
public partial class CreateApplicationUserTelephonePage : ComponentBase
{
    #region Properties

    public CreateApplicationUserTelephoneRequest InputModel { get; set; } = new();

    #endregion

    #region Parameters

    [Parameter] public IMask BrazilPhoneNumberMask { get; set; } = new PatternMask("(00) 00000-0000");

    #endregion

    #region Services

    [Inject] public IApplicationUserTelephoneHandler TelephoneHandler { get; set; } = null!;

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides
    
    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Novo Telefone");
        await StartService.ValidateAccesByTokenAsync();
        InputModel.Type = ETypeOfPhone.Celular;
    }
    
    #endregion
    
    #region Methods

    public async Task OnValidSubmitAsync()
    {
        try
        {
            InputModel.UserId = StartService.GetSelectedUserId();
            var result = await TelephoneHandler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message!, Severity.Success);
                NavigationService.NavigateTo("/usuarios/telefones");
            }
            else
                Snackbar.Add(result.Message!, Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    #endregion
}