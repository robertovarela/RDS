using Blazored.LocalStorage;
using RDS.Core.Services;

namespace RDS.Web.Services
{
    public class ManipulateUserStateValuesService(
        UserStateService userState,
        ILocalStorageService localStorage,
        TokenService tokenService)
    {
        public async Task<long> SetDefaultValues()
        {
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
    }
}