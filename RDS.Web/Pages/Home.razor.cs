namespace RDS.Web.Pages;

public class HomePage : ComponentBase
{
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    //private string roles = "";
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        StartService.SetPageTitle("RDS - Desenvolvimento de SoftWares");
        //await StartService.ValidateAccesByToken();
        StartService.SetDefaultValues();
        StateHasChanged();
        
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        // if (user.Identity != null && user.Identity.IsAuthenticated)
        // {
        //     var rolesList = user.Claims
        //         .Where(c => c.Type == ClaimTypes.Role)
        //         .Select(c => c.Value)
        //         .ToList();
        //     roles = string.Join(", ", rolesList);
        //
        //     Console.WriteLine($"Roles do usuário autenticado: {roles}");
        // }
        // else
        // {
        //     roles = "Usuário não autenticado";
        //     Console.WriteLine(roles);
        // }
        //
        // if (user.Identity != null && user.Identity.IsAuthenticated)
        // {
        //     var roles = user.Claims
        //         .Where(c => c.Type.StartsWith("custom_role_"))
        //         .Select(c => c.Value)
        //         .ToList();
        //
        //     Console.WriteLine($"Roles do usuário autenticado: {string.Join(", ", roles)}");
        // }
    }

    #endregion
}