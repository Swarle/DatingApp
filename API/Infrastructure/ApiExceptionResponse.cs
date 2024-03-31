using System.Net;

namespace API.Infrastructure;

public class ApiExceptionResponse
{
    public ApiExceptionResponse(HttpStatusCode statusCode, string? message = null, string? details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public string? Details { get; set; }
}