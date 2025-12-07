
using System.Security.Claims;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.JwtServices
{
    public interface IJwtService
    {
        public string GenerateJwtToken(Account _account);
        public int? ValidateToken(string token);
        public ClaimsPrincipal ValidateTokenClaimsPrincipal(string token);
        public string RefeshToken(string email);
        string GenerateEmailVerificationToken(string email);
    }
}