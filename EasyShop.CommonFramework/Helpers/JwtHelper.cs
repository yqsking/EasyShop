using EasyShop.CommonFramework.Exception;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EasyShop.CommonFramework.Helpers
{
    /// <summary>
    /// Jwt工具类
    /// </summary>
    public static class JwtHelper
    {
      
        /// <summary>
        /// 生成Token字符串
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public static string GetToken(IConfiguration configuration,params Claim[] claim)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString("N")),//令牌编号
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),//令牌颁发时间
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),//令牌生效时间
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),//令牌过期时间
                new Claim(JwtRegisteredClaimNames.Iss,configuration["Authentication:JwtBearer:Issuer"]), // 签发者
                new Claim(JwtRegisteredClaimNames.Aud,configuration["Authentication:JwtBearer:Audience"]) // 接收者
            };
            if(claim!=null)
            {
                claims.AddRange(claim);
            }
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]));//秘钥
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwt = new JwtSecurityToken(
              claims: claims,
              signingCredentials: creds
              );
            string token = "Bearer" + " " + new JwtSecurityTokenHandler().WriteToken(jwt);
            return  token;


        }

        /// <summary>
        /// 验证Token有效性
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CheckToken(IConfiguration configuration, string token)
        {
            if(token.IsNull())
            {
                throw new CustomException("当前账户登录无效！");
            }
            return true;
        }
    }
}
