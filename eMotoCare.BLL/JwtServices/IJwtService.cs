using eMotoCare.DAL.Entities;
using System.Security.Claims;

namespace eMotoCare.BLL.JwtServices
{
    public interface IJwtService
    {
        public string GenerateJwtToken(Account _account);
        public int? ValidateToken(string token);
        public ClaimsPrincipal ValidateTokenClaimsPrincipal(string token);
        public string GetAccountId();
        public string GetEmail();
        public string GetRole();
        public string GetTokenId();
        DateTime GetExpire(string token);
        Guid GetAccountIdGuid();
        public string RefeshToken(string email);

    }
}
