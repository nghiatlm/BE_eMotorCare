using System.Text.Json;
using eMotoCare.API.Extensions;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.ApiResponse;
using Microsoft.IdentityModel.Tokens;

namespace eMotoCare.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == 401)
                {
                    await HandleUnauthorizedResponse(context);
                }
                else if (context.Response.StatusCode == 403)
                {
                    await HandleForbiddenResponse(context);
                }
            }
            catch (AppException ex)
            {
                await HandleAppException(context, ex);
            }
            catch (SecurityTokenExpiredException)
            {
                await HandleAppException(context, new AppException(ErrorCode.TOKEN_EXPIRED));
            }
            catch (SecurityTokenInvalidAudienceException)
            {
                await HandleAppException(context, new AppException(ErrorCode.INVALID_TOKEN));
            }
            catch (SecurityTokenInvalidIssuerException)
            {
                await HandleAppException(context, new AppException(ErrorCode.INVALID_TOKEN));
            }
            catch (SecurityTokenInvalidSigningKeyException)
            {
                await HandleAppException(context, new AppException(ErrorCode.INVALID_TOKEN));
            }
            catch (SecurityTokenValidationException)
            {
                await HandleAppException(context, new AppException(ErrorCode.INVALID_TOKEN));
            }
            catch (UnauthorizedAccessException)
            {
                await HandleAppException(context, new AppException(ErrorCode.UNAUTHORIZED));
            }
        }

        private async Task HandleAppException(HttpContext context, AppException ex)
        {
            _logger.LogError(ex, "An application exception occurred: {ErrorCode}", ex.ErrorCode);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.ErrorCode.GetStatusCode();
            var response = new ApiResponse
            {
                Code = (int)ex.ErrorCode.GetStatusCode(),
                Success = false,
                Message = ex.ErrorCode.GetMessage(),
                Data = null,
            };
            var json = JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
            await context.Response.WriteAsync(json);
        }

        private async Task HandleUnauthorizedResponse(HttpContext context)
        {
            await HandleAppException(context, new AppException(ErrorCode.UNAUTHORIZED));
        }

        private async Task HandleForbiddenResponse(HttpContext context)
        {
            await HandleAppException(context, new AppException(ErrorCode.FORBIDDEN));
        }
    }
}
