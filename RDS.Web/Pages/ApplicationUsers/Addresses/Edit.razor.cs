namespace RDS.Web.Pages.ApplicationUsers.Addresses;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EditApplicationUserAddressPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    protected UpdateApplicationUserAddressRequest InputModel { get; set; } = new();
    private long LoggedUserId { get; } = StartService.GetLoggedUserId();
    protected long UserId { get; } = StartService.GetSelectedUserId();
    private bool IsAdmin { get; } = StartService.GetIsAdmin();
    private bool IsOwner { get; } = StartService.GetIsOwner();
    private long AddressId { get; } = StartService.GetSelectedAddressId();
    protected bool IsNotEdit { get; set; }
    protected const string BackUrl = "/usuarios/enderecos";

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

    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IApplicationUserAddressHandler AddressHandler { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Editar Endereço");
        await StartService.ValidateAccesByTokenAsync();

        LoadStartValues();
        await LoadUserAddressAsync();
    }

    #endregion

    #region Methods

    private void LoadStartValues()
    {
        if (UserId == 0 || AddressId == 0)
        {
            NavigationService.NavigateTo(BackUrl);
        }
        if (!IsAdmin)
        {
            IsNotEdit = IsOwner && (LoggedUserId != UserId);
        }
    }
    private async Task LoadUserAddressAsync()
    {
        IsBusy = true;
        try
        {
            var requestAddress = new GetApplicationUserAddressByIdRequest
            {
                Id = AddressId
            };
            var responseAddress = await AddressHandler.GetByIdAsync(requestAddress);
            if (responseAddress is { IsSuccess: true, Data: not null })
            {
                InputModel = new UpdateApplicationUserAddressRequest
                {
                    CompanyId = responseAddress.Data.UserId,
                    Id = responseAddress.Data.Id,
                    PostalCode = responseAddress.Data.PostalCode,
                    Street = responseAddress.Data.Street,
                    Number = responseAddress.Data.Number,
                    Complement = responseAddress.Data.Complement ?? string.Empty,
                    Neighborhood = responseAddress.Data.Neighborhood,
                    City = responseAddress.Data.City,
                    State = responseAddress.Data.State,
                    Country = responseAddress.Data.Country,
                    TypeOfAddress = responseAddress.Data.TypeOfAddress
                };
            }
            else
            {
                Snackbar.Add("Endereço não encontrado", Severity.Warning);
                NavigationService.NavigateTo(BackUrl);
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
    
    protected async void OnUpdateButtonClickedAsync()
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir o endereço será atualizado. Deseja continuar?",
            yesText: "ALTERAR",
            cancelText: "Cancelar");

        if (result is true)
            await OnValidSubmitAsync();

        StateHasChanged();
    }
    
    private async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await AddressHandler.UpdateAsync(InputModel);

            if (result.IsSuccess && result.Data is not null)
            {
                Snackbar.Add("Endereço atualizado", Severity.Success);
            }
            else
            {
                Snackbar.Add($"O endereço {InputModel.CompanyId} - {InputModel.Id} - {InputModel.PostalCode} não foi encontrado", Severity.Warning);

            }

            NavigationService.NavigateTo(BackUrl);
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