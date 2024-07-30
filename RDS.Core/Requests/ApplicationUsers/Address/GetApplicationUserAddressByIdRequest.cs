namespace RDS.Core.Requests.ApplicationUsers.Address;

public abstract class GetApplicationUserAddressByIdRequest : Request
{
    public long Id { get; set; } 
}