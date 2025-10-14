using System.Net;
using eMotoCare.Common.Exceptions;

namespace eMotoCare.API.Extensions
{
    public static class ErrorCodeExtensions
    {
        private static readonly Dictionary<ErrorCode, ErrorCodeMetadata> _metadata =
            new()
            {
                {
                    ErrorCode.UNAUTHORIZED,
                    new(
                        HttpStatusCode.Unauthorized,
                        "Unauthorized access. Please provide valid authentication credentials."
                    )
                },
                {
                    ErrorCode.FORBIDDEN,
                    new(
                        HttpStatusCode.Forbidden,
                        "Forbidden. You do not have permission to access this resource."
                    )
                },
                {
                    ErrorCode.TOKEN_EXPIRED,
                    new(HttpStatusCode.Unauthorized, "Token has expired. Please login again.")
                },
                {
                    ErrorCode.INVALID_TOKEN,
                    new(HttpStatusCode.Unauthorized, "Invalid authentication token.")
                },
                {
                    ErrorCode.INVALID_CREDENTIALS,
                    new(HttpStatusCode.Unauthorized, "Invalid credentials provided.")
                },
                { ErrorCode.USER_NOT_FOUND, new(HttpStatusCode.NotFound, "User not found.") },
                {
                    ErrorCode.INTERNAL_SERVER_ERROR,
                    new(
                        HttpStatusCode.InternalServerError,
                        "An unexpected error occurred. Please try again later."
                    )
                },
                {
                    ErrorCode.EMAIL_ALREADY_EXISTS,
                    new(HttpStatusCode.BadRequest, "Email already exists.")
                },
                { ErrorCode.LIST_EMPTY, new(HttpStatusCode.NotFound, "No users found.") },
                {
                    ErrorCode.EMAIL_DO_NOT_EXISTS,
                    new(HttpStatusCode.BadRequest, "Email do not exist.")
                },
                { ErrorCode.INVALID_PASSWORD, new(HttpStatusCode.BadRequest, "invalid password.") },
                {
                    ErrorCode.TOKEN_NOT_NULL,
                    new(HttpStatusCode.BadRequest, "Token must be not null.")
                },
                {
                    ErrorCode.ACCOUNT_STATUS_NOT_NULL,
                    new(HttpStatusCode.BadRequest, "Account status must be not null.")
                },
                {
                    ErrorCode.ROLE_NOT_NULL,
                    new(HttpStatusCode.BadRequest, "Role must be not null.")
                },
                { ErrorCode.ROLE_HAS_EXISTED, new(HttpStatusCode.NotFound, "Role has exited") },
                {
                    ErrorCode.NOT_FOUND,
                    new ErrorCodeMetadata(HttpStatusCode.NotFound, "Not found")
                },
                {
                    ErrorCode.INVALID_ACCOUNT_ROLE,
                    new(HttpStatusCode.BadRequest, "Invalid account role.")
                },
                { ErrorCode.INVALID_ACCOUNT, new(HttpStatusCode.BadRequest, "Invalid account.") },
                { ErrorCode.HAS_INACTIVE, new(HttpStatusCode.BadRequest, "Status inactive") },
                { ErrorCode.HAS_EXISTED, new(HttpStatusCode.BadRequest, "Has existed") },
                { ErrorCode.NOT_NULL, new(HttpStatusCode.BadRequest, "Must be not null") },
                {
                    ErrorCode.PAYMENT_VERIFICATION_FAILED,
                    new(
                        HttpStatusCode.BadRequest,
                        "Payment verification failed. Please check your payment details and try again."
                    )
                },
            };

        public static string GetMessage(this ErrorCode code) => _metadata[code].Message;

        public static HttpStatusCode GetStatusCode(this ErrorCode code) =>
            _metadata[code].StatusCode;
    }
}
