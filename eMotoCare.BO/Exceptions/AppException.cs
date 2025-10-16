
using System.Net;

namespace eMotoCare.BO.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public object? Details { get; }

        public AppException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, object? details = null)
            : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}