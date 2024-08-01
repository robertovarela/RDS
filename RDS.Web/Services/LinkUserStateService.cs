using RDS.Core.Services;
 
namespace RDS.Web.Services
{
    public class LinkUserStateService(UserStateService userState)
    {
        public void LinkToUrlUser(string url = "", long userId = 0)
        {
            userState.SetSelectedUserId(userId);
            NavigationService.NavigateTo(url);
        }
        
        public void LinkToUrlAddress(string url = "", long userId = 0, long addressId = 0)
        {
            userState.SetSelectedUserId(userId);
            userState.SetSelectedAddressId(addressId);
            NavigationService.NavigateTo(url);
        }
        
        public void LinkToUrlCategory(string url = "", long userId = 0, long categoryId = 0)
        {
            userState.SetSelectedUserId(userId);
            userState.SetSelectedCategoryId(categoryId);
            NavigationService.NavigateTo(url);
        }
    }
}