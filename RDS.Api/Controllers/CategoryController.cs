namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/categories")]
public class CategoryController(AppDbContext context) : ControllerBase
{
    [HttpPost("create")]
    public async Task<Response<Category?>> CreateAsync([FromBody] CreateCategoryRequest request)
    {
        try
        {
            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 201, "Categoria criada com sucesso!");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Não foi possível criar a categoria");
        }
    }

    [HttpPut("update")]
    public async Task<Response<Category?>> UpdateAsync([FromBody] UpdateCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(null, 404, "Categoria não encontrada");

            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria atualizada com sucesso");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Não foi possível alterar a categoria");
        }
    }

    [HttpDelete("delete")]
    public async Task<Response<Category?>> DeleteAsync([FromBody] DeleteCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(null, 404, "Categoria não encontrada");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria excluída com sucesso!");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Não foi possível excluir a categoria");
        }
    }

    [HttpPost("all")]
    public async Task<PagedResponse<List<Category>>> GetAllAsync([FromBody] GetAllCategoriesRequest request)
    {
        try
        {
            var query = context
                .Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>>(
                categories,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Category>>(null, 500, "Não foi possível consultar as categorias");
        }
    }

    [HttpPost("byid")]
    public async Task<Response<Category?>> GetByIdAsync([FromBody] GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return category is null
                ? new Response<Category?>(null, 404, "Categoria não encontrada")
                : new Response<Category?>(category);
        }
        catch
        {
            return new Response<Category?>(null, 500, "Não foi possível recuperar a categoria");
        }
    }
}