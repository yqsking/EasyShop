using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace EasyShop.Api.Filters
{
    /// <summary>
    /// 权限认证过滤器
    /// </summary>
    public class AuthorizeFilter: ActionFilterAttribute
    {
        /// <summary>
        /// 登录授权验证
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //验证当前api是否有匿名登录
            if (context.Filters.Any(x => x.GetType().Equals(typeof(AnonymousAttribute))))
            {
                return;
            }
            //try
            //{
               
            //    var authorization = context.HttpContext.Request.Headers["authorization"];
            //    if (string.IsNullOrEmpty(authorization) || string.IsNullOrEmpty(authorization.FirstOrDefault()))
            //    {
            //        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //        context.Result = new JsonResult(ApiResultUnit.Error<string>("token不能为空"));
            //        return;
            //    }
            //    var token = authorization.FirstOrDefault()?.Replace("Bearer ", "");

            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            //    var claims = jwtSecurityToken.Claims;
            //    var user = claims.FirstOrDefault(m => m.Type == "sub").Value;
            //    var type = claims.FirstOrDefault(m => m.Type == "type").Value;
            //    //判断当前登陆账号是否可以访问api
            //    var data = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(item => item.GetType() == typeof(UserRootAttribute));
            //    if (data != null)
            //    {
            //        if (!Convert.ToInt32((data as UserRootAttribute).UserType).ToString().Equals(type))
            //        {
            //            context.Result = new JsonResult(ApiResultUnit.Error<string>("当前登录账号角色暂无调用此功能权限"));
            //            return;
            //        }
            //    }
            //    context.HttpContext.Session.SetInt32(ApiConsts.UserIdKey, int.Parse(user));
            //    context.HttpContext.Session.SetInt32(ApiConsts.UserType, int.Parse(type));
            //}
            //catch (Exception ex)
            //{
            //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    context.Result = new JsonResult(ApiResultUnit.Error<string>(ex.Message));
            //}
        }
    }
}
