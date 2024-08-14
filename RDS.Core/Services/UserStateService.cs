namespace RDS.Core.Services
{
    public class UserStateService
    {
        private string? UrlOrigen { get; set; }
        private long LoggedUserId { get; set; }
        private long SelectedUserId { get; set; }
        private long SelectedUserAddressId { get; set; }
        private long SelectedCategoryId { get; set; }
        
        public string GetUrlOrigen() => UrlOrigen ?? string.Empty;
        public long GetLoggedUserId() => LoggedUserId;
        public long GetSelectedUserId() => SelectedUserId;
        public long GetSelectedAddressId() => SelectedUserAddressId;
        public long GetSelectedCategoryId() => SelectedCategoryId;

        public void SetUrlOrigen(string url) => UrlOrigen = url;
        public void SetLoggedUserId(long loggedUserId) => LoggedUserId = loggedUserId;
        public void SetSelectedUserId(long userId) => SelectedUserId = userId;
        public void SetSelectedAddressId(long addressId) => SelectedUserAddressId = addressId;
        public void SetSelectedCategoryId(long categoryId) => SelectedCategoryId = categoryId;
    }
}