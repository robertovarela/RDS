namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/categories")]
public class CategoryController(ICategoryHandler categoryHandler) : ControllerBase
{
    [HttpPost("create")]
    public async Task<Response<Category?>> CreateAsync([FromBody] CreateCategoryRequest request)
    {
        return await categoryHandler.CreateAsync(request);
    }

    [HttpPut("update")]
    public async Task<Response<Category?>> UpdateAsync([FromBody] UpdateCategoryRequest request)
    {
        return await categoryHandler.UpdateAsync(request);
    }

    [HttpDelete("delete")]
    public async Task<Response<Category?>> DeleteAsync([FromBody] DeleteCategoryRequest request)
    {
        return await categoryHandler.DeleteAsync(request);
    }

    [HttpPost("all")]
    public async Task<PagedResponse<List<Category>>> GetAllAsync([FromBody] GetAllCategoriesRequest request)
    {
        return await categoryHandler.GetAllAsync(request);
    }

    [HttpPost("byid")]
    public async Task<Response<Category?>> GetByIdAsync([FromBody] GetCategoryByIdRequest request)
    {
        return await categoryHandler.GetByIdAsync(request);
    }
}