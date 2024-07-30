namespace RDS.Core.Requests;

public class PagedRequestApplicationUser
{
    public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
    public int PageSize { get; set; } = Configuration.DefaultPageSize;
}