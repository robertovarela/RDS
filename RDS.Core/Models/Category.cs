namespace RDS.Core.Models;

public class Category
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long CompanyId { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}