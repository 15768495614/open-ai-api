using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenAiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [Route("[action]")]
        [HttpPost]
        public async Task<string> Login([FromQuery] string username, [FromQuery] string password)
        {
            if (username != "user" || password != "pwd")
            {
                return "";
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "u_admin"), //HttpContext.User.Identity.Name
                new Claim(ClaimTypes.Role, "r_admin"), //HttpContext.User.IsInRole("r_admin")
                new Claim(JwtRegisteredClaimNames.Jti, "admin"),
                new Claim("UserId","1"),
                new Claim("AssignKey","key12346")
            };

            // 2. 从 appsettings.json 中读取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("124678787456asdf"));

            // 3. 选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;

            // 4. 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            // 5. 根据以上，生成token
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "https://localhost:7047",     //Issuer
                audience: "https://localhost:7047",   //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddHours(1),    //expires
                signingCredentials               //Credentials
            );

            // 6. 将token变为string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return await Task.FromResult(token);
        }
    }
}
