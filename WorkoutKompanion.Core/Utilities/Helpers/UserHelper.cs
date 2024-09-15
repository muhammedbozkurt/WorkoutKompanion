using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace WorkoutKompanion.Core.Utilities.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserName()
        {
            // JWT token'dan kullanıcı adını al
            var userName = GetUserNameFromToken();

            // Eğer kullanıcı adı yoksa "KompanionSystem" olarak döner
            return string.IsNullOrEmpty(userName) ? "KompanionSystem" : userName;
        }

        private string GetUserNameFromToken()
        {
            // HttpContext üzerinden Authorization Header'ını alıyoruz
            var accessToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(accessToken))
                return null; // Eğer token yoksa null döner

            // Token'ı çözümlemek için JwtSecurityTokenHandler kullanıyoruz
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            // Kullanıcı adı "sub" veya "name" claim'inden alınır
            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
                           ?? jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

            return userName;
        }
    }
}
