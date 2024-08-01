using Microsoft.AspNetCore.Components;

namespace RDS.Web
{
    public class NavigationService
    {
        private static NavigationManager? _navigationManager;

        public static void Initialize(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public static void NavigateTo(string url)
        {
            _navigationManager?.NavigateTo(url);
        }
        
        public static void NavigateToLogin()
        {
            _navigationManager?.NavigateTo("/sair");
            _navigationManager?.NavigateTo("/login");
        }
        
        public static void NavigateToRegister()
        {
            _navigationManager?.NavigateTo("/sair");
            _navigationManager?.NavigateTo("/registrar");
        }

    }
}