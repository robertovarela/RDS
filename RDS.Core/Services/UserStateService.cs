﻿using RDS.Core.Models.ViewModels.Company;

namespace RDS.Core.Services
{
    public class UserStateService
    {
        private string? PageTitle { get; set; } = string.Empty;
        private List<string> SourceUrl { get; set; } = [];
        private string? CurrentUrl { get; set; } = string.Empty;
        
        private bool IsAdmin { get; set; } = false;
        private bool IsOwner { get; set; } = false;
        private long LoggedUserId { get; set; }
        private long SelectedUserId { get; set; }
        private string SelectedUserName { get; set; } = string.Empty;
        private long SelectedUserAddressId { get; set; }
        private long SelectedUserTelephoneId { get; set; }
        private long SelectedCompanyId { get; set; }
        private List<CompanyIdNameViewModel> UserCompanies { get; set; } = [];
        private long SelectedCategoryId { get; set; }
        private long SelectedTransactionId { get; set; }
        
        public string GetPageTitle() => PageTitle ?? string.Empty;
        public List<string> GetSourceUrl() => SourceUrl;
        public string GetCurrentUrl() => CurrentUrl ?? string.Empty;
        public bool GetIsAdmin() => IsAdmin;
        public bool GetIsOwner() => IsOwner;
        public long GetLoggedUserId() => LoggedUserId;
        public long GetSelectedUserId() => SelectedUserId;
        public string GetSelectedUserName() => SelectedUserName;
        public long GetSelectedAddressId() => SelectedUserAddressId;
        public long GetSelectedTelephoneId() => SelectedUserTelephoneId;
        public long GetSelectedCompanyId() => SelectedCompanyId;
        public List<CompanyIdNameViewModel> GetUserCompanies() => UserCompanies;
        public long GetSelectedCategoryId() => SelectedCategoryId;
        public long GetSelectedTransactionId() => SelectedTransactionId;

        public void SetPageTitle(string title) => PageTitle = title;
        public void SetSourceUrl(List<string> urlList) => SourceUrl = urlList;
        public void SetCurrentUrl(string url) => CurrentUrl = url;
        public void SetIsAdmin(bool isAdmin) => IsAdmin = isAdmin;
        public void SetIsOwner(bool isOwner) => IsOwner = isOwner;
        public void SetLoggedUserId(long loggedUserId) => LoggedUserId = loggedUserId;
        public void SetSelectedUserId(long userId) => SelectedUserId = userId;
        public void SetSelectedUserName(string userName) => SelectedUserName = userName;
        public void SetSelectedAddressId(long addressId) => SelectedUserAddressId = addressId;
        public void SetSelectedTelephoneId(long telephoneId) => SelectedUserTelephoneId = telephoneId;
        public void SetSelectedCompanyId(long companyId) => SelectedCompanyId = companyId;
        public void SetUserCompanies(List<CompanyIdNameViewModel> companiesId) => UserCompanies = companiesId;
        public void SetSelectedCategoryId(long categoryId) => SelectedCategoryId = categoryId;
        public void SetSelectedTransactionId(long transactionId) => SelectedTransactionId = transactionId;
    }
}