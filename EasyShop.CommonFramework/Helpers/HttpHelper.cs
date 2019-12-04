using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace EasyShop.CommonFramework.Helpers
{
    /// <summary>
    /// Http请求工具类
    /// </summary>
    public static  class HttpHelper
    {
        /// <summary>
        /// 发送http get请求
        /// </summary>
        /// <param name="url"></param>
        public static string SendGet(string url, IDictionary<string, string> parameters = null)
        {
            if (parameters != null)
            {
                url += "?" + BuildParameter(parameters);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ReadWriteTimeout = 5000;
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            //返回内容
            string result = myStreamReader.ReadToEnd();
            return result;
        }
        /// <summary>
        /// 发送http post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string SendPost(string url, IDictionary<string, string> parameters = null)
        {
            byte[] postData = Encoding.UTF8.GetBytes(BuildParameter(parameters));
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.KeepAlive = false;
            req.ProtocolVersion = HttpVersion.Version10;
            req.Timeout = 5000;
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            req.ContentLength = postData.Length;

            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            //返回内容
            string result = myStreamReader.ReadToEnd();
            return result;

        }

        /// <summary>
        /// 下载远程文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] DownloadFile(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();

                List<byte> btlst = new List<byte>();
                int b = responseStream.ReadByte();
                while (b > -1)
                {
                    btlst.Add((byte)b);
                    b = responseStream.ReadByte();
                }
                byte[] bytes = btlst.ToArray();
                return bytes;
            }

        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        private static string BuildParameter(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    hasParam = true;
                }
            }
            return postData.ToString();
        }
    }
}
