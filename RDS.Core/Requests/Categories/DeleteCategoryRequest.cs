namespace RDS.Core.Requests.Categories;

public abstract class DeleteCategoryRequest : Request
{
    public long Id { get; set; }
}