namespace RDS.Web.Pages;

public class HomePage : ComponentBase
{
    #region Services
    
    [Inject] private TokenService TokenService { get; set; } = null!;
    [Inject] private HttpClientService HttpClientService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;
    [Inject] private ManipulateUserStateValuesService ManipulateUserStateValues { get; set; } = null!;
    [Inject] public UserStateService UserState { get; set; } = null!;
    [Inject] public IApplicationUserHandler UserHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    // protected override async Task OnInitializedAsync()
    // {
    //     var userId = await ManipulateUserStateValues.SetDefaultValues();
    // }

    #endregion
}