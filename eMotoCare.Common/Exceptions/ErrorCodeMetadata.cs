using System.Net;

namespace eMotoCare.Common.Exceptions
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
