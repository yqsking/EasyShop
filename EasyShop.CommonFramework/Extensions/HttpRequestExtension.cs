using Microsoft.AspNetCore.Http;
using System.Text;

namespace EasyShop.CommonFramework.Extensions
{
    /// <summary>
    /// HttpRequest 的扩展
    /// </summary>
    public static  class HttpRequestExtension
    {
        /// <summary>
        /// 获取当前访问的URL路径
        /// </summary>
        /// <param name="request">HTTP请求信息</param>
        /// <returns>请求的URL路径</returns>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }
    }
}
