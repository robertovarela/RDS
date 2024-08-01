using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RDS.Core.Handlers;
using RDS.Core.Requests.ApplicationUsers;
using RDS.Core.Services;
using RDS.Web.Common;
using RDS.Web.Components.Informations;
using RDS.Web.Services;

namespace RDS.Web.Pages.ApplicationUsers;

public partial class EditApplicationUsersPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public UpdateApplicationUserRequest InputModel { get; set; } = new();

    public MudDatePicker _picker = new ();
    public DateTime minDate = DateTime.Today.AddYears(-110);
    public DateTime maxDate = DateTime.Today.AddYears(-12);
    public string Url { get; set; } = "/usuarios/enderecos";

    #endregion

    #region Parameters

    [Parameter]
    public IMask BrazilPostalCode { get; set; } = new PatternMask("00000-000");

    #endregion

    #region Services

    [Inject] private TokenService TokenService { get; set; } = null!;
    [Inject] private HttpClientService HttpClientService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;
    [Inject] private ManipulateUserStateValuesService ManipulateUserStateValues { get; set; } = null!;
    [Inject] public UserStateService UserState { get; set; } = null!;
    [Inject] public IApplicationUserHandler UserHandler { get; set; } = null!;
    
    [Inject] public IApplicationUserAddressHandler AddressHandler { get; set; } = null!;
    [Inject] public LinkUserStateService Link { get; set; } = null!;
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    [Inject] public IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var userId = await ManipulateUserStateValues.GetSelectedUserId();
        IsBusy = true;
        
        try
        {
            var request = new GetApplicationUserByIdRequest{UserId = userId};
            var response = await UserHandler.GetByIdAsync(request);
            if (response is { IsSuccess: true, Data: not null })
                InputModel = new UpdateApplicationUserRequest
                {
                    UserId = response.Data.Id,
                    Name = response.Data.Name ?? string.Empty,
                    Email = response.Data.Email ?? string.Empty,
                    Cpf = response.Data.Cpf ?? string.Empty,
                    BirthDate = response.Data.BirthDate
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

    protected override void OnAfterRender(bool firstRender)
    {
        if (string.IsNullOrEmpty(InputModel.BirthDate.ToString()))
        {
            _picker.GoToDate(maxDate, false);
        }
    }
    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        if (InputModel.Cpf == string.Empty)
        {
            InputModel.Cpf = null;
        }

        try
        {
            var result = await UserHandler.UpdateAsync(InputModel);

            if (result.IsSuccess && result.Data is not null)
            {
                Snackbar.Add("Usuário atualizado", Severity.Success);
            }
            else
            {
                Snackbar.Add($"O usuário {InputModel.UserId}-{InputModel.Name} não foi encontrado", Severity.Warning);

            }

            NavigationService.NavigateTo("/");
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