using EasyShop.Appliction.ViewModels;
using EasyShop.CommonFramework.Exception;
using EasyShop.CommonFramework.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyShop.Api.MiddleWare
{
    /// <summary>
    /// 全局异常处理中间件
    /// </summary>
    public  class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare> logger)      
        {
            _next = next;
            _logger = logger;
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            //基于guid生成一个唯一标识
            var errorId = Guid.NewGuid().ToString();
            var requestToString = await context.Request.RequestStringAsync();
            try
            {
                await _next.Invoke(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex, errorId, requestToString);
            }
        }

        /// <summary>
        /// 处理异常信息
        /// </summary>
        private Task HandleExceptionAsync(HttpContext context, Exception ex, string errorId, string requestString)
        {
           if(ex.GetType()!=typeof(CustomException))
           {
                //不属于自定义异常，记录错误日志
                var message = new StringBuilder();
                message.AppendLine("【异常ID】：" + errorId);
                message.AppendLine("【异常信息】：" + ex.Message);
                message.AppendLine("【请求参数】：");
                message.AppendLine(requestString);
                message.AppendLine("【堆栈调用】：" + ex.StackTrace);
                _logger.LogError(message.ToString());
            }
            var result= new ApiResult<BaseResponseDto> {ErrorId=errorId,IsSuccess=false,Message=ex.Message,Data=default };
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

       
    }
}
