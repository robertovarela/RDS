namespace RDS.Core.Services
{
    public class UserStateService
    {
        private string? PageTitle { get; set; } = string.Empty;
        private List<string> SourceUrl { get; set; } = [];
        private string? CurrentUrl { get; set; } = string.Empty;
        private long LoggedUserId { get; set; }
        private long SelectedUserId { get; set; }
        private string SelectedUserName { get; set; } = string.Empty;
        private long SelectedUserAddressId { get; set; }
        private long SelectedCompanyId { get; set; }
        private long SelectedCategoryId { get; set; }
        private long SelectedTransactionId { get; set; }
        
        public string GetPageTitle() => PageTitle ?? string.Empty;
        public List<string> GetSourceUrl() => SourceUrl;
        public string GetCurrentUrl() => CurrentUrl ?? string.Empty;
        public long GetLoggedUserId() => LoggedUserId;
        public long GetSelectedUserId() => SelectedUserId;
        public string GetSelectedUserName() => SelectedUserName;
        public long GetSelectedAddressId() => SelectedUserAddressId;
        public long GetSelectedCompanyId() => SelectedCompanyId;
        public long GetSelectedCategoryId() => SelectedCategoryId;
        public long GetSelectedTransactionId() => SelectedTransactionId;

        public void SetPageTitle(string title) => PageTitle = title;
        public void SetSourceUrl(List<string> urlList) => SourceUrl = urlList;
        public void SetCurrentUrl(string url) => CurrentUrl = url;
        public void SetLoggedUserId(long loggedUserId) => LoggedUserId = loggedUserId;
        public void SetSelectedUserId(long userId) => SelectedUserId = userId;
        public void SetSelectedUserName(string userName) => SelectedUserName = userName;
        public void SetSelectedAddressId(long addressId) => SelectedUserAddressId = addressId;
        public void SetSelectedCompanyId(long companyId) => SelectedCompanyId = companyId;
        public void SetSelectedCategoryId(long categoryId) => SelectedCategoryId = categoryId;
        public void SetSelectedTransactionId(long transactionId) => SelectedTransactionId = transactionId;
    }
}