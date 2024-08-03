namespace RDS.Core.Requests;

public abstract class PagedRequest : Request
{
    public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
    public int PageSize { get; set; } = Configuration.DefaultPageSize;
    public string SearchTerm { get; set; } = string.Empty;
}