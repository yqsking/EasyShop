using EasyShop.Api.JWT.Requirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
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
        public IAuthenticationSchemeProvider Schemes { get; set; }

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
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                //验证签发的用户信息
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (result.Succeeded)
                {
                  
                    httpContext.User = result.Principal;
                    //判断是否过期
                    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.UtcNow)
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
