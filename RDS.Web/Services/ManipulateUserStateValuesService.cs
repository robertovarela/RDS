namespace RDS.Web.Services
{
    public class ManipulateUserStateValuesService(
        UserStateService userState,
        ILocalStorageService localStorage,
        TokenService tokenService,
        ISnackbar snackbar)
    {
        public void SetDefaultValues()
        {
            userState.SetSelectedUserId(userState.GetLoggedUserId());
            userState.SetSelectedAddressId(0);
            userState.SetSelectedCategoryId(0);
        }
        
        public async Task<string> ValidateAccessByToken()
        {
            string token = await localStorage.GetItemAsync<string>("authToken")??string.Empty;
            long userId = 0;
            if (!string.IsNullOrEmpty(token))
            {
                if (long.TryParse(tokenService.GetUserIdFromToken(token), out var id))
                {
                    userId = id;
                }
            }
            
            userState.SetLoggedUserId(userId);
            if (userId == 0)
            {
                snackbar.Add("Token expirado ou inválido!", Severity.Warning);
                NavigationService.NavigateToLogin();
            }

            return token;
        }
        
        public long GetLoggedUserId()
        {
            long loggedUserId = userState.GetLoggedUserId();
            return loggedUserId;
        }
        
        public long GetSelectedUserId()
        {
            long selectedUserId = userState.GetSelectedUserId();
            if (selectedUserId != 0) return selectedUserId;
            userState.SetSelectedUserId(userState.GetLoggedUserId());
            selectedUserId = userState.GetSelectedUserId();
            return selectedUserId;
        }
        
        public long GetSelectedAddressId()
        {
            long selectedAddressId = userState.GetSelectedAddressId();

            return selectedAddressId;
        }
        
        public long GetSelectedCategoryId()
        {
            long selectedCategoryId = userState.GetSelectedCategoryId();

            return selectedCategoryId;
        }
        
        public void SetSelectedUserId(long userId)
        {
            userState.SetSelectedUserId(userId);
        }
        
        public void SetSelectedAddressId(long addressId)
        {
            userState.SetSelectedAddressId(addressId);
        }
        
        public void SetSelectedCategoryId(long categoryId)
        {
            userState.SetSelectedCategoryId(categoryId);
        }
    }
}