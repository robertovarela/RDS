namespace RDS.Core.Responses;

public class Response<TData>
{
    public int StatusCode { get; private set; }

    [JsonConstructor]
    public Response(
        TData? data = default,
        int statusCode = Configuration.DefaultStatusCode,
        string? message = null)
    {
        Data = data;
        StatusCode = statusCode;
        Message = message;
    }

    public TData? Data { get; set; }
    public string? Message { get; set; }

    [JsonIgnore] public bool IsSuccess => StatusCode >= 200 && StatusCode <= 299;
}
// public class Response<TData>
// {
//     private readonly int _code;
//
//     [JsonConstructor]
//     public Response() => _code = Configuration.DefaultStatusCode;
//
//     public Response(
//         TData? data,
//         int code = Configuration.DefaultStatusCode,
//         string? message = null)
//     {
//         Data = data;
//         Message = message;
//         _code = code;
//     }
//
//     public TData? Data { get; set; }
//     public string? Message { get; set; }
//
//     [JsonIgnore]
//     public bool IsSuccess
//         => _code is >= 200 and <= 299;
//     
//     public int StatusCode => _code;
// }