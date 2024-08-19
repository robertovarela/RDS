namespace RDS.Core.Models;

public class Company
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public long UserId { get; set; }
}