namespace RDS.Web.Pages.Transactions;


// ReSharper disable once PartialTypeWithSinglePart
public partial class CreateTransactionPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    public CreateTransactionRequest InputModel { get; set; } = new();
    public List<Category> Categories { get; set; } = [];
    private long UserId { get; set; }

    #endregion

    #region Services

    [Inject] public ITransactionHandler TransactionHandler { get; set; } = null!;

    [Inject] public ICategoryHandler CategoryHandler { get; set; } = null!;

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Novo Lançamento");
        await StartService.ValidateAccesByToken();
        UserId = StartService.GetSelectedUserId();
        IsBusy = true;

        try
        {
            var request = new GetAllCategoriesRequest
            {
                UserId = UserId
            };
            var result = await CategoryHandler.GetAllAsync(request);
            if (result.IsSuccess)
            {
                Categories = result.Data ?? [];
                InputModel.CategoryId = Categories.FirstOrDefault()?.Id ?? 0;
                StateHasChanged();
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

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await TransactionHandler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationService.NavigateTo("/lancamentos/historico");
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


    public IEnumerable<string> ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            yield return "O valor deve ser maior do que zero.";
        }
    }
    #endregion
}