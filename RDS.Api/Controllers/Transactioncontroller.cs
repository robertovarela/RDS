using RDS.Core.Requests.Transactions;

namespace RDS.Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("v1/transactions")]
public class Transactioncontroller(ITransactionHandler transactionHandler) : ControllerBase
{
     [HttpPost("create")]
    public async Task<Response<Transaction?>> CreateAsync([FromBody] CreateTransactionRequest request)
    {
        return await transactionHandler.CreateAsync(request);
    }

    [HttpPut("update")]
    public async Task<Response<Transaction?>> UpdateAsync([FromBody] UpdateTransactionRequest request)
    {
        return await transactionHandler.UpdateAsync(request);
    }

    [HttpDelete("delete")]
    public async Task<Response<Transaction?>> DeleteAsync([FromBody] DeleteTransactionRequest request)
    {
        return await transactionHandler.DeleteAsync(request);
    }
    
    [HttpPost("byid")]
    public async Task<Response<Transaction?>> GetByIdAsync([FromBody] GetTransactionByIdRequest request)
    {
        return await transactionHandler.GetByIdAsync(request);
    }

    [HttpPost("byperiod")]
    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync([FromBody] GetTransactionsByPeriodRequest request)
    {
        return await transactionHandler.GetByPeriodAsync(request);
    }
}