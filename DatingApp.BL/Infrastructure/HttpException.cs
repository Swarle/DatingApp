using System.Net;

namespace DatingApp.BL.Infrastructure
{
    public class HttpException : Exception
    {
        public HttpException(HttpStatusCode statusCode, string? message = null, string? details = null)
        : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }

        public HttpStatusCode StatusCode { get; set; }
        public string? Details { get; set; }
    }
}
