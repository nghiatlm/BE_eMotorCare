
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eMotoCare.BO.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BE_eMotoCare.API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly JwtSettings _jwtSettings;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger, IOptions<JwtSettings> jwtOptions)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
            _jwtSettings = jwtOptions.Value;
            _jwtSettings.SecretKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? _jwtSettings.SecretKey;
            _jwtSettings.Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? _jwtSettings.Issuer;
            _jwtSettings.Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? _jwtSettings.Audience;

            string? envExpires = Environment.GetEnvironmentVariable("JWT_EXPIRES_IN_MINUTES");
            if (int.TryParse(envExpires, out int expiresInMinutes))
            {
                _jwtSettings.ExpiresInMinutes = expiresInMinutes;
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            _logger.LogInformation($"Processing request for path: {path}");

            if (path?.StartsWith("/public") == true)
            {
                _logger.LogInformation("Public endpoint, skipping JWT validation");
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            string token = null;

            if (!string.IsNullOrEmpty(authHeader))
            {
                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authHeader.Substring("Bearer ".Length).Trim();
                }
                else
                {
                    token = authHeader.Trim();
                }
            }

            if (token != null)
            {
                _logger.LogInformation("JWT token found in request");
                AttachUserToContext(context, token);
            }
            else
            {
                _logger.LogWarning("No JWT token found in request");
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _jwtSettings.SecretKey;
                var key = Encoding.UTF8.GetBytes(secretKey);
                var issuer = _jwtSettings.Issuer;
                var audience = _jwtSettings.Audience;
                _logger.LogInformation("Attempting to validate JWT token");

                var validationParameters = new TokenValidationParameters
                {
                    RoleClaimType = ClaimTypes.Role,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidIssuer = issuer,
                    ValidateAudience = false,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken)
                {
                    _logger.LogError("Invalid JWT token format");
                    throw new SecurityTokenException("Invalid JWT token");
                }

                _logger.LogInformation($"JWT token validated successfully. User roles: {string.Join(", ", principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value))}");
                context.User = principal;
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogError("JWT token has expired");
                throw;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError($"JWT token validation failed: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in JWT Middleware: {ex.Message}");
                throw;
            }
        }
    }
}