using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace EasyShop.Api.Filter
{
    /// <summary>
    /// dto参数验证过滤器
    /// </summary>
    public class VerificationFilter : IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        /// <summary>
        /// dto模型参数验证
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorList = context.ModelState.SelectMany(item => item.Value.Errors.Select(args => args.ErrorMessage)).ToList();
                if(errorList.Any())
                {
                    throw new Exception(errorList.FirstOrDefault());
                }
            }
        }
    }
}
