namespace RDS.Core.Services
{
    public class UserStateService
    {
        public long LoggedUserId { get; private set; }
        public long SelectedUserId { get; set; }
        public long SelectedUserAddressId { get; set; }
        public long SelectedCategoryId { get; set; }
        
        public long GetLoggedUserId() => LoggedUserId;

        public long GetSelectedUserId() => SelectedUserId;

        public long GetSelectedAddressId() => SelectedUserAddressId;

        public long GetSelectedCategoryId() => SelectedCategoryId;

        public void SetLoggedUserId(long loggedUserId) => LoggedUserId = loggedUserId;

        public void SetSelectedUserId(long userId) => SelectedUserId = userId;

        public void SetSelectedAddressId(long addressId) => SelectedUserAddressId = addressId;

        public void SetSelectedCategoryId(long categoryId) => SelectedCategoryId = categoryId;
    }
}