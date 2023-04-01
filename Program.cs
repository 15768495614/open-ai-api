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
            //ע�����
            service.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true, //�Ƿ���֤Issuer
                    ValidIssuer = "https://localhost:7047", //������Issuer
                    ValidateAudience = true, //�Ƿ���֤Audience
                    ValidAudience = "https://localhost:7047", //������Audience
                    ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("124678787456asdf")), //SecurityKey
                    ValidateLifetime = true, //�Ƿ���֤ʧЧʱ��
                    ClockSkew = TimeSpan.FromMinutes(1), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
                    RequireExpirationTime = true,
                };
            });
        }
    }


    
}