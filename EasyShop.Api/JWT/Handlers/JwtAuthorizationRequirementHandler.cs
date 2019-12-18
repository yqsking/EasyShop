using EasyShop.Api.JWT.Requirements;
using EasyShop.CommonFramework.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyShop.Api.JWT.Handlers
{
    /// <summary>
    /// 授权处理程序
    /// </summary>
    public class JwtAuthorizationRequirementHandler : AuthorizationHandler<JwtAuthorizationRequirement>
    {
        /// <summary>
        /// 授权方式（cookie, bearer, oauth, openid）
        /// </summary>
        private readonly IAuthenticationSchemeProvider _provider;

        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public JwtAuthorizationRequirementHandler(IAuthenticationSchemeProvider provider,IConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        /// <summary>
        /// 处理授权
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtAuthorizationRequirement requirement)
        {
            var httpContext = (context.Resource as AuthorizationFilterContext).HttpContext;
            //获取授权方式
            var defaultAuthenticate = await _provider.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                //验证签发的用户信息
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (result.Succeeded)
                {
        
                    httpContext.User = result.Principal;
                    context.Succeed(requirement);
                    string newToken = JwtHelper.RefreshTokenExpTime(_configuration, result.Principal.Claims.ToList());
                    //Response返回最新jwt token
                    httpContext.Response.Headers.Add("Authorization",newToken);
                    return;
                }
            }
            context.Fail();
        }
    }
}
