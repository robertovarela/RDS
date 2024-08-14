﻿namespace RDS.Core.Services
{
    public class UserStateService
    {
        private string? UrlOrigen { get; set; }
        private long LoggedUserId { get; set; }
        private long SelectedUserId { get; set; }
        
        private string SelectedUserName { get; set; } = string.Empty;
        private long SelectedUserAddressId { get; set; }
        private long SelectedCategoryId { get; set; }
        
        public string GetUrlOrigen() => UrlOrigen ?? string.Empty;
        public long GetLoggedUserId() => LoggedUserId;
        public long GetSelectedUserId() => SelectedUserId;
        public string GetSelectedUserName() => SelectedUserName;
        public long GetSelectedAddressId() => SelectedUserAddressId;
        public long GetSelectedCategoryId() => SelectedCategoryId;

        public void SetUrlOrigen(string url) => UrlOrigen = url;
        public void SetLoggedUserId(long loggedUserId) => LoggedUserId = loggedUserId;
        public void SetSelectedUserId(long userId) => SelectedUserId = userId;
        public void SetSelectedUserName(string userName) => SelectedUserName = userName;
        public void SetSelectedAddressId(long addressId) => SelectedUserAddressId = addressId;
        public void SetSelectedCategoryId(long categoryId) => SelectedCategoryId = categoryId;
    }
}