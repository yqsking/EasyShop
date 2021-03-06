﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        /// <summary>
        /// 获取请求日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<string> RequestStringAsync(this HttpRequest request)
        {
            var message = new StringBuilder();
            if (request.Method != null)
            {
                message.AppendLine($"[Method]: {request.Method} ");
            }
            message.AppendLine($"[RequestUri]: {request.GetAbsoluteUri()} ");

            //头部Header
            if (request.Headers != null)
            {
                message.AppendLine("[Header] Values: ");
                foreach (var headerItem in request.Headers)
                {
                    message.AppendLine($"--> [{headerItem.Key}]: {headerItem.Value} ");
                }
            }
            //Body
            if (!string.IsNullOrWhiteSpace(request.Method) && !request.Method.ToUpper().Equals("GET") && !request.Headers["Content-Type"].ToString().ToLower().Equals("multipart/form-data"))
            {
                request.EnableBuffering();

                await request.Body.DrainAsync(CancellationToken.None);
                request.Body.Seek(0L, SeekOrigin.Begin);

                var bodyContent = string.Empty;

                await using (var ms = new MemoryStream(2048))
                {
                    request.Body.Position = 0;
                    request.Body.CopyTo(ms);
                    var content = ms.ToArray();
                    bodyContent = Encoding.UTF8.GetString(content);
                }

                message.AppendLine("[Body]: ");
                message.AppendLine($"--> {bodyContent}");

                request.Body.Seek(0L, SeekOrigin.Begin);
            }

            return message.ToString();
        }
    }
}
