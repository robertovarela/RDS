namespace RDS.Web.Pages.Categories;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EditCategoryPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; set; }
    public UpdateCategoryRequest InputModel { get; set; } = new();
    private long UserId { get; set; }
    private long Id { get; set; }

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public ICategoryHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Editar Categoria");
        await StartService.ValidateAccesByToken();
        UserId = StartService.GetSelectedUserId();
        Id = StartService.GetSelectedCategoryId();
        //StateHasChanged();
 
        try
        {
            var request = new GetCategoryByIdRequest
            {
                Id = Id,
                UserId = UserId
            };

            IsBusy = true;

            var response = await Handler.GetByIdAsync(request);
            if (response is { IsSuccess: true, Data: not null })
                InputModel = new UpdateCategoryRequest
                {
                    Id = response.Data.Id,
                    Title = response.Data.Title,
                    Description = response.Data.Description
                };
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
        InputModel.Id = Id;
        InputModel.UserId = UserId;
        
        try
        {
            var result = await Handler.UpdateAsync(InputModel);

            if (result is { IsSuccess: true, Data: not null })
            {
                Snackbar.Add("Categoria atualizada", Severity.Success);
            }
            else
            {
                Snackbar.Add($"A categoria {InputModel.Id}-{InputModel.Title} não foi encontrada", Severity.Warning);
            }

            NavigationService.NavigateTo("/categorias");
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