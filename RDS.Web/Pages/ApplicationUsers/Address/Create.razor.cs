using Microsoft.AspNetCore.Components;
using MudBlazor;
using RDS.Core.Handlers;
using RDS.Core.Requests.ApplicationUsers.Address;

namespace RDS.Web.Pages.ApplicationUsers.Address
{
    public partial class CreateApplicationUserAddresPage : ComponentBase
    {
        #region Properties

        public bool IsBusy { get; set; } = false;
        public CreateApplicationUserAddressRequest InputModel { get; set; } = new();

        #endregion

        #region Parameters

        [Parameter]
        public IMask BrazilPostalCode { get; set; } = new PatternMask("00000-000");

        #endregion

        #region Services

        [Inject]
        public IApplicationUserAddressHandler AddressHandler { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        #endregion

        #region Methods

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
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
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
