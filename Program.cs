using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OpenAiApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            ConfigureJwt(builder.Services, builder.Configuration);

            builder.Services.AddControllers();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureJwt(IServiceCollection service, IConfiguration configuration)
        {
            //注册服务
            service.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true, //是否验证Issuer
                    ValidIssuer = "https://localhost:7047", //发行人Issuer
                    ValidateAudience = true, //是否验证Audience
                    ValidAudience = "https://localhost:7047", //订阅人Audience
                    ValidateIssuerSigningKey = true, //是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("124678787456asdf")), //SecurityKey
                    ValidateLifetime = true, //是否验证失效时间
                    ClockSkew = TimeSpan.FromMinutes(1), //过期时间容错值，解决服务器端时间不同步问题（秒）
                    RequireExpirationTime = true,
                };
            });
        }
    }


    
}