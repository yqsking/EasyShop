using EasyShop.CommonFramework.Exception;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        /// <param name="configuration">配置文件</param>
        /// <param name="customClaims">自定义的有效载荷（包含当前登录的用户信息）</param>
        /// <returns></returns>
        public static string GetToken(IConfiguration configuration, params Claim[] customClaims)
        {
            int outTimeMinute = configuration["Authentication:JwtBearer:OutTimeMinute"].ToInt();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString("N")),//令牌编号
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),//令牌颁发时间
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToUniversalTime().ToString()),//令牌生效时间
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(outTimeMinute).ToUniversalTime().ToString()),//令牌过期时间
                new Claim(JwtRegisteredClaimNames.Iss,configuration["Authentication:JwtBearer:Issuer"]), // 签发者
                new Claim(JwtRegisteredClaimNames.Aud,configuration["Authentication:JwtBearer:Audience"]) // 接收者
            };
            if (customClaims != null)
            {
                claims.AddRange(customClaims);
            }
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]));//秘钥
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return CreateToken(claims,creds);


        }


        /// <summary>
        /// 更新Token过期时间
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string RefreshTokenExpTime(IConfiguration configuration, List<Claim> claims)
        {
            if (claims == null)
            {
                throw new CustomException("当前登录用户Token无法更新！");
            }
            int outTimeMinute = configuration["Authentication:JwtBearer:OutTimeMinute"].ToInt();
            var expClaim = new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(outTimeMinute)).ToUnixTimeSeconds()}");//令牌过期时间
            claims.RemoveAll(item => item.Type == JwtRegisteredClaimNames.Exp);//移除token原来的过期时间有效载荷
            claims.Add(expClaim);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]));//秘钥
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return CreateToken(claims,creds);
        }


        public static string OutTimeTokenExpTime(IConfiguration configuration,List<Claim> claims)
        {
            var exp= claims.FirstOrDefault(item => item.Type == JwtRegisteredClaimNames.Exp).Value;
            return string.Empty;
        }


        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="signingCredentials"></param>
        /// <returns></returns>
        private static string CreateToken(List<Claim> claims, SigningCredentials signingCredentials)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
             claims: claims,
             signingCredentials: signingCredentials
             );
            string token = "Bearer" + " " + new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;

        }
    }
}
