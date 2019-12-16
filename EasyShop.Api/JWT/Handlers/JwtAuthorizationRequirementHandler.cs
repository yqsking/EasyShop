using EasyShop.Api.JWT.Requirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public JwtAuthorizationRequirementHandler(IAuthenticationSchemeProvider provider)
        {
            _provider = provider;
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
                    string exp = httpContext.User.Claims.SingleOrDefault(item => item.Type == JwtRegisteredClaimNames.Exp).Value;//Unix时间戳
                    var expDateTime= DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc).AddSeconds(exp.ToInt()).ToLocalTime();
                    //判断是否过期
                    if (expDateTime >= DateTime.Now)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                       // await httpContext.Response.WriteAsync(JsonHelper.ObjectToJSON(BaseResponse.CreateForbidden()));
                        context.Fail();
                    }
                    return;
                }
            }
            context.Fail();
        }
    }
}
