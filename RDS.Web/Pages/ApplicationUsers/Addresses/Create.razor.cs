﻿namespace RDS.Web.Pages.ApplicationUsers.Addresses;

// ReSharper disable once PartialTypeWithSinglePart
public partial class CreateApplicationUserAddressPage : ComponentBase
{
    #region Properties

    protected CreateApplicationUserAddressRequest InputModel { get; set; } = new();
    private long UserId { get; } = StartService.GetSelectedUserId();
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

    [Inject] private IApplicationUserAddressHandler AddressHandler { get; set; } = null!;

    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides
    
    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("Novo Endereço");
        await StartService.ValidateAccesByTokenAsync();
        
        LoadStartValues();
    }
    
    #endregion
    
    #region Methods

    private void LoadStartValues()
    {
        if (UserId == 0)
        {
            NavigationService.NavigateTo(BackUrl);
        }

        if (UserId != StartService.GetLoggedUserId())
        {
            IsNotEdit = true;
        }
    }
    public async Task OnValidSubmitAsync()
    {
        try
        {
            InputModel.CompanyId = StartService.GetSelectedUserId();
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
    }

    #endregion
}