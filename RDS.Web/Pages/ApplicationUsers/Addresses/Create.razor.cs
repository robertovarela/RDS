namespace RDS.Web.Pages.ApplicationUsers.Addresses;

public partial class CreateApplicationUserAddressPage : ComponentBase
{
    #region Properties

    public CreateApplicationUserAddressRequest InputModel { get; set; } = new();

    #endregion

    #region Parameters

    [Parameter] public IMask BrazilPostalCode { get; set; } = new PatternMask("00000-000");
    [Parameter] public List<string> Estados { get; set; } = new List<string>
    {
        "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
        "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
        "RS", "RO", "RR", "SC", "SP", "SE", "TO"
    };

    #endregion

    #region Services

    [Inject] public IApplicationUserAddressHandler AddressHandler { get; set; } = null!;

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides
    
    protected override async Task OnInitializedAsync()
    {
        await StartService.ValidateAccesByToken();
    }
    
    #endregion
    
    #region Methods

    public async Task OnValidSubmitAsync()
    {
        try
        {
            InputModel.UserId = StartService.GetSelectedUserId();
            var result = await AddressHandler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationService.NavigateTo("/usuarios/enderecos");
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