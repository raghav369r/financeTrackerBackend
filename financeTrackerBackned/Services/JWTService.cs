using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using financeTrackerBackned.Domain;
using Microsoft.IdentityModel.Tokens;

namespace financeTrackerBackned.Services
{
  public class JWTService
  {
    private readonly IConfiguration _configuration;
    private readonly string jwtKey;
    private readonly string jwtIssuer;
    private readonly string jwtAudience;

    public JWTService(IConfiguration configuration)
    {
      _configuration = configuration;
      jwtKey = _configuration["Jwt:Key"] ?? "";
      jwtIssuer = _configuration["Jwt:Issuer"] ?? "";
      jwtAudience = _configuration["Jwt:Audience"] ?? "";

    }
    public string GenerateJwtToken(User user)
    {
      var claims = new[]
      {
            new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(user.Id)),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role,"User")
        };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: jwtIssuer,
          audience: jwtAudience,
          claims: claims,
          expires: DateTime.Now.AddHours(24),
          signingCredentials: creds);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
// add IHttpContextProvider to DI 
// var user = _httpContextAccessor.HttpContext?.User;
// user.Identity.IsAuthenticated

// user.FindFirst(ClaimTypes.NameIdentifier)?.Value
//                        ?? user.FindFirst("sub")?.Value;

// user.FindFirst(ClaimTypes.Email)?.Value 
//                      ?? user.FindFirst("email")?.Value;

// string? role = user.FindFirst(ClaimTypes.Role)?.Value
//             ?? user.FindFirst("role")?.Value;

// _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
