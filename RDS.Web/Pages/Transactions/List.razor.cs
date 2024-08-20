namespace RDS.Web.Pages.Transactions;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ListTransactionsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    protected List<Transaction> Transactions { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;
    protected int CurrentYear { get; set; } = DateTime.Now.Year;
    public int CurrentMonth { get; set; } = DateTime.Now.Month;
    protected int[] Years { get; set; } = LoadYears();
    protected long UserId { get; set; }
    protected const string EditUrl = "/lancamentos/editar";


    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    [Inject] public IDialogService DialogService { get; set; } = null!;

    [Inject] public ITransactionHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Lançamentos");
        await StartService.ValidateAccesByToken();
        UserId = StartService.GetSelectedUserId();
        await GetTransactionsAsync();
    }

    #endregion

    #region Public Methods

    public async Task OnSearchAsync()
    {
        await GetTransactionsAsync();
        StateHasChanged();
    }

    #endregion

    #region Private Methods

    private async Task GetTransactionsAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetTransactionsByPeriodRequest
            {
                CompanyId = UserId,
                StartDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                EndDate = DateTime.Now.GetLastDay(CurrentYear, CurrentMonth),
                PageNumber = 1,
                PageSize = 1000
            };
            var result = await Handler.GetByPeriodAsync(request);
            if (result.IsSuccess)
                Transactions = result.Data ?? [];
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

    private async Task OnDeleteAsync(long id, string title)
    {
        IsBusy = true;

        try
        {
            var result = await Handler.DeleteAsync(new DeleteTransactionRequest { Id = id });
            if (result.IsSuccess)
            {
                Snackbar.Add($"Lançamento {title} removido!", Severity.Success);
                Transactions.RemoveAll(x => x.Id == id);
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
    
    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir o lançamento {title} será excluído. Esta ação é irreversível! Deseja continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id, title);

        StateHasChanged();
    }

    public Func<Transaction, bool> Filter => transaction =>
    {
        if (string.IsNullOrEmpty(SearchTerm))
            return true;

        return transaction.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
               || transaction.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
    };
    
    private static int[] LoadYears()
    {
        int period = 10;
        int yearsToAdd = 3;
        var years = new int[period];
        for (int i = 0; i < period; i++)
        {
            years[i] = DateTime.Now.AddYears(yearsToAdd - i).Year;
        }
        
        return years;
    }

    #endregion
}