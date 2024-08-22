namespace RDS.Core.Requests.Companies;

public class CreateCompanyUserRequest : Request
{
    public long OwnerId { get; set; }
    public bool IsAdmin { get; set; }
}