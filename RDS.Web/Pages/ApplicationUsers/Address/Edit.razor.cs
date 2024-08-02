﻿namespace RDS.Web.Pages.ApplicationUsers.Address;

public class EditApplicationUserAddressPage : ComponentBase
{
    #region Properties

    protected bool IsBusy { get; private set; }
    public UpdateApplicationUserAddressRequest InputModel { get; set; } = new();

    #endregion

    #region Parameters

    [Parameter]
    public IMask BrazilPostalCode { get; set; } = new PatternMask("00000-000");
    
    [Parameter] public List<string> Estados { get; set; } = new List<string>
    {
        "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
        "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
        "RS", "RO", "RR", "SC", "SP", "SE", "TO"
    };

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IApplicationUserAddressHandler AddressHandler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var userId = await StartService.GetSelectedUserId();
            var id = await StartService.GetSelectedAddressId();
 
            var requestAddress = new GetApplicationUserAddressByIdRequest
            {
                UserId = userId ,
                Id = id
            };
            var responseAddress = await AddressHandler.GetByIdAsync(requestAddress);
            if (responseAddress is { IsSuccess: true, Data: not null })
            {
                InputModel = new UpdateApplicationUserAddressRequest
                {
                    UserId = responseAddress.Data.UserId,
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
                Thread.Sleep(2000);
                NavigationService.NavigateTo("/usuarios/enderecos");
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
            var result = await AddressHandler.UpdateAsync(InputModel);

            if (result.IsSuccess && result.Data is not null)
            {
                Snackbar.Add("Endereço atualizado", Severity.Success);
            }
            else
            {
                Snackbar.Add($"O endereço {InputModel.UserId} - {InputModel.Id} - {InputModel.PostalCode} não foi encontrado", Severity.Warning);

            }

            NavigationService.NavigateTo("/usuarios/enderecos");
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