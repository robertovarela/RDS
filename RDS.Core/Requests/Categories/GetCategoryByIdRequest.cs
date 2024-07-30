namespace RDS.Core.Requests.Categories;

public abstract class GetCategoryByIdRequest : Request
{
    public long Id { get; set; }
}