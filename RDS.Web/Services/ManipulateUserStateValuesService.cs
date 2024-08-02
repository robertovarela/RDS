using Blazored.LocalStorage;
using RDS.Core.Services;

namespace RDS.Web.Services
{
    public class ManipulateUserStateValuesService(
        UserStateService userState,
        ILocalStorageService localStorage,
        TokenService tokenService)
    {
        public async Task<long> SetDefaultValues(bool disableNavigation = false)
        {
            if(disableNavigation) return 0;
            
            long userId = await GetUserLoggedIdFromToken();
            userState.SetLoggedUserId(userId);
            userState.SetSelectedUserId(userId);
            userState.SetSelectedAddressId(0);
            userState.SetSelectedCategoryId(0);

            return userId;
        }

        private async Task<long> GetUserLoggedIdFromToken()
        {
            long userId = 0;
            var token = await localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                if (long.TryParse(tokenService.GetUserIdFromToken(token), out var id))
                {
                    userId = id;
                }
            }

            if (userId == 0)
            {
                NavigationService.NavigateToLogin();
            }

            return userId;
        }
        
        public async Task<long> GetLoggedUserId()
        {
            long loggedUserId = userState.GetLoggedUserId();
            if (loggedUserId != 0) return loggedUserId;
            loggedUserId = await SetDefaultValues();
            NavigationService.NavigateTo("/");

            return loggedUserId;
        }
        public async Task<long> GetSelectedUserId()
        {
            long selectedUserId = userState.GetSelectedUserId();
            if (selectedUserId != 0) return selectedUserId;
            selectedUserId = await SetDefaultValues();
            NavigationService.NavigateTo("/");

            return selectedUserId;
        }
        
        public async Task<long> GetSelectedAddressId()
        {
            long selectedAddressId = userState.GetSelectedAddressId();
            if (selectedAddressId != 0) return selectedAddressId;
            selectedAddressId = await SetDefaultValues();
            NavigationService.NavigateTo("/");

            return selectedAddressId;
        }
        
        public async Task<long> GetSelectedCategoryId()
        {
            long selectedCategoryId = userState.GetSelectedCategoryId();
            if (selectedCategoryId != 0) return selectedCategoryId;
            selectedCategoryId = await SetDefaultValues(true);
            NavigationService.NavigateTo("/");

            return selectedCategoryId;
        }
    }
}