namespace RDS.Web.Services
{
    public class LinkUserStateService
    {
        public void LinkToUrlUser(string url = "", long userId = 0)
        {
            StartService.SetSelectedUserId(userId);
            NavigationService.NavigateTo(url);
        }
        
        public void LinkToUrlAddress(string url = "", string urlOrigen = "", long userId = 0, long addressId = 0)
        {
            StartService.SetSelectedUserId(userId);
            StartService.SetSelectedAddressId(addressId);
            NavigationService.NavigateTo(url);
        }
        
        public void LinkToUrlCategory(string url = "", long userId = 0, long categoryId = 0)
        {
            StartService.SetSelectedUserId(userId);
            StartService.SetSelectedCategoryId(categoryId);
            NavigationService.NavigateTo(url);
        }
    }
}