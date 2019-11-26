using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyShop.Api.Filters
{
    /// <summary>
    /// 匿名登录过滤器
    /// </summary>
    public  class AnonymousAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
