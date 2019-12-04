using System.Linq;

namespace System
{
    /// <summary>
    /// Url地址拓展
    /// </summary>
    public static  class HttpUrlExtension
    {
        /// <summary>
        /// 获取Url上一级路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetParentUrl(this string url)
        {
            string newUrl = string.Empty;
            string[] temp = url.Split('\\');
            if(temp.Any())
            {
                newUrl = string.Join("\\", temp.Take(temp.Length - 1));
            }
            return newUrl;
        }

    }
}
