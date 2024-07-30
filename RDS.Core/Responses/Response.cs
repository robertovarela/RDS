namespace RDS.Core.Responses;

public class Response<TData>
{
    private readonly int _code;

    [JsonConstructor]
    public Response() => _code = Configuration.DefaultStatusCode;

    public Response(
        TData? data,
        int code = Configuration.DefaultStatusCode,
        string? message = null)
    {
        Data = data;
        Message = message;
        _code = code;
    }

    public TData? Data { get; set; }
    public string? Message { get; set; }

    [JsonIgnore]
    public bool IsSuccess
        => _code is >= 200 and <= 299;
    
    public int StatusCode => _code;
}

// using System.Text.Json;
// using System.Text.Json.Serialization;
//
// namespace RDS.Core.Responses;
//
// [JsonConverter(typeof(ResponseJsonConverter))]
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
//     [JsonIgnore] public bool IsSuccess => _code is >= 200 and <= 299;
//
//     public int StatusCode => _code;
// }
//
// public class ResponseJsonConverter : JsonConverter<Response<object>>
// {
//     public override Response<object>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
//         {
//             var root = doc.RootElement;
//             var data = root.GetProperty("data").GetRawText();
//             var message = root.GetProperty("message").GetString();
//             var statusCode = root.GetProperty("statusCode").GetInt32();
//
//             return new Response<object>(
//                 JsonSerializer.Deserialize<object>(data, options),
//                 statusCode,
//                 message);
//         }
//     }
//
//     public override void Write(Utf8JsonWriter writer, Response<object> value, JsonSerializerOptions options)
//     {
//         writer.WriteStartObject();
//         writer.WritePropertyName("data");
//         JsonSerializer.Serialize(writer, value.Data, options);
//         writer.WriteString("message", value.Message);
//         writer.WriteNumber("statusCode", value.StatusCode);
//         writer.WriteEndObject();
//     }
// }