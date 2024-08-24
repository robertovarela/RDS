namespace RDS.Core.Models.ViewModels.ApplicationUser;

public class AllUsersViewModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}