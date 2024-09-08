namespace RDS.Web.Pages.ApplicationUsers;

public partial class EditMyAccountPage : ComponentBase
{
    protected override void OnInitialized()
    {
        StartService.SetSelectedUserId(StartService.GetLoggedUserId());
        NavigationService.NavigateTo("/usuarios/editar");
    }
}