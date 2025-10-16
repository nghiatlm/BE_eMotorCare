
using System.Net;

namespace eMotoCare.BO.DTO.ApiResponse
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        private ApiResponse(HttpStatusCode statusCode, bool success, string? message = default, T? data = default)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Data = data;
        }

        private ApiResponse(string message)
        {
            Message = message;
        }

        public static ApiResponse<T> SuccessResponse(T? data = default, string message = "Request successful")
        {
            return new ApiResponse<T>(HttpStatusCode.OK, true, message, data);
        }

        public static ApiResponse<T> CreatedSuccess(T? data = default, string message = "Resource created")
        {
            return new ApiResponse<T>(HttpStatusCode.Created, true, message, data);
        }

        public static ApiResponse<T> BadRequest(string message = "BadRequest")
        {
            return new ApiResponse<T>(HttpStatusCode.BadRequest, false, message);
        }
    }
}