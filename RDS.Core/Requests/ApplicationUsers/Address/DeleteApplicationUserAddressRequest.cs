namespace RDS.Core.Requests.ApplicationUsers.Address;
    public abstract class DeleteApplicationUserAddressRequest : Request
    {
        public long Id { get; set; }
    }