namespace RDS.Web.Pages.Categories;

public class CreateCategoryPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    public CreateCategoryRequest InputModel { get; set; } = new();
    private long UserId { get; set; }

    #endregion

    #region Services

    [Inject] public ICategoryHandler CategoryHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Nova Categoria");
        await StartService.ValidateAccesByToken();
        UserId = StartService.GetSelectedUserId();
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        InputModel.CompanyId = UserId;
        try
        {
            var result = await CategoryHandler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationService.NavigateTo("/categorias");
            }
            else
                Snackbar.Add(result.Message, Severity.Error);
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