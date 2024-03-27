using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.BL.Infrastructure
{
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string message = null, string details = null)
        : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }

        public HttpStatusCode StatusCode { get; set; }
        public string? Details { get; set; }
        public object? Result { get; set; }
    }
}
