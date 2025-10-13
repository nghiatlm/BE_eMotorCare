using System.Net;

namespace eMotoCare.Application.Exceptions
{
    public class ErrorCodeMetadata
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }

        public ErrorCodeMetadata(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
