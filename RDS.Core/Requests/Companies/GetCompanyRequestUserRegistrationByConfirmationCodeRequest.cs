namespace RDS.Core.Requests.Companies;

public class GetCompanyRequestUserRegistrationByConfirmationCodeRequest : Request
{
    public string ConfirmationCode { get; set; } = null!;
}