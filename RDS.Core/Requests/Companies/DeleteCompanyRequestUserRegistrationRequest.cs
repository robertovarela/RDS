namespace RDS.Core.Requests.Companies;

public class DeleteCompanyRequestUserRegistrationRequest : Request
{
    public long Id { get; set; }
    public bool IsOwner { get; set; }
}