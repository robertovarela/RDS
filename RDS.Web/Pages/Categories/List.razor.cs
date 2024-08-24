namespace RDS.Web.Pages.Categories;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ListCategoriesPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; set; }
    protected List<Category> Categories { get; set; } = [];
    private long CompanyId { get; set; }
    protected string SearchTerm { get; set; } = string.Empty;
    protected const string AddUrl = "/categorias/adicionar";
    protected const string Url = "/categorias/editar";
    protected const string UrlOrigen = "/categorias";

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public ICategoryHandler CategoryHandler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Categorias");
        await StartService.ValidateAccesByTokenAsync();
        CompanyId = StartService.GetSelectedUserId();
        await LoadCategoriesAsync();
    }

    #endregion

    #region Methods

    private async Task LoadCategoriesAsync()
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
                Categories = result.Data ?? [];
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
            $"Ao prosseguir a categoria ( {id} - {title} ) será excluída. Esta é uma ação irreversível! Deseja continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id);

        StateHasChanged();
    }

    private async Task OnDeleteAsync(long id)
    {
        try
        {
            var request = new DeleteCategoryRequest
            {
                Id = id,
                CompanyId = CompanyId
            };
            var result = await CategoryHandler.DeleteAsync(request);
            if (result.IsSuccess)
                Categories.RemoveAll(x => x.Id == id);
            Snackbar.Add(result.Message, result.Data != null ? Severity.Success : Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    public Func<Category, bool> Filter => category =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;

        if (category.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (category.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (category.Description is not null &&
            category.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    #endregion
}