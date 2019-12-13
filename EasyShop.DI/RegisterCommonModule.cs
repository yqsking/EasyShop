using EasyShop.BasicImpl.DBContext;
using EasyShop.BasicImpl.Repositorys;
using EasyShop.CommonFramework.Exception;
using EasyShop.Dommain.Repositorys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入公共服务
    /// </summary>
    public static class RegisterCommonModule
    {
        /// <summary>
        /// 依赖注入公共服务
        /// </summary>
        /// <param name="collection"></param>
        public static void RegisterCommon(this IServiceCollection collection)
        {
            collection.AddScoped(typeof(DbContext),typeof(EasyShopDBContext));
            collection.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));

        }

        /// <summary>
        /// 依赖注入JWT授权
        /// </summary>
        /// <param name="collection"></param>
        public static void RegisterJWT(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", options =>
            {
                options.Audience = configuration["Authentication:JwtBearer:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),
                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],
                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = configuration["Authentication:JwtBearer:Audience"],
                    // Validate the token expiry
                    ValidateLifetime = true,
                    // If you want to allow a certain amount of clock drift, set that here
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    //此处为权限验证失败后触发的事件
                    OnChallenge = context =>
                    {
                        //此处代码为终止.Net Core默认的返回类型和数据结果
                        context.HandleResponse();
                        throw new CustomException("抱歉，当前账户登录授权失败！");
                    }
                };
            });

        }
    }
}
