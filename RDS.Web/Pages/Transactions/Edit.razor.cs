namespace RDS.Web.Pages.Transactions;

public partial class EditTransactionPage : ComponentBase
{
    #region Properties
    
    public bool IsBusy { get; set; } = false;
    public UpdateTransactionRequest InputModel { get; set; } = new();
    protected List<Category> Categories { get; set; } = [];
    protected long UserId { get; set; }
    private long CompanyId { get; set; }
    private long TransactionId { get; set; }

    #endregion

    #region Services

    [Inject]
    public ITransactionHandler TransactionHandler { get; set; } = null!;

    [Inject]
    public ICategoryHandler CategoryHandler { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Editar Lançamento");
        await StartService.ValidateAccesByToken();
        UserId = StartService.GetSelectedUserId();
        CompanyId = StartService.GetSelectedCompanyId();
        TransactionId = StartService.GetSelectedTransactionId();
        IsBusy = true;

        await GetTransactionByIdAsync();
        await GetCategoriesAsync();

        if(InputModel.Amount < 0) 
            InputModel.Amount *= -1;

        StateHasChanged();
        IsBusy = false;
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            InputModel.CompanyId = CompanyId;
            var result = await TransactionHandler.UpdateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add("Lançamento atualizado", Severity.Success);
                NavigationService.NavigateTo("/lancamentos/historico");
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
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

    #region Private Methods

    private async Task GetTransactionByIdAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetTransactionByIdRequest
            {
                Id = TransactionId,
                CompanyId = CompanyId
            };
            var result = await TransactionHandler.GetByIdAsync(request);
            if (result is { IsSuccess: true, Data: not null })
            {
                InputModel = new UpdateTransactionRequest
                {
                    CategoryId = result.Data.CategoryId,
                    PaidOrReceivedAt = result.Data.PaidOrReceivedAt,
                    Title = result.Data.Title,
                    Type = result.Data.Type,
                    Amount = result.Data.Amount,
                    Id = result.Data.Id,
                };
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

    private async Task GetCategoriesAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllCategoriesRequest
            {
                CompanyId = CompanyId
            };
            var result = await CategoryHandler.GetAllAsync(request);
            if (result.IsSuccess)
            {
                Categories = result.Data ?? [];
                //InputModel.CategoryId = Categories.FirstOrDefault()?.Id ?? 0;
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
}