using Newtonsoft.Json;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// String拓展
    /// </summary>
    public static  class StringExtension
    {
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNull(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 将字符串反序列化成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToModel<T>(this string str) where T:new ()
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 将字符串反序列化成集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string str) where T:new()
        {
            return JsonConvert.DeserializeObject<List<T>>(str);
        }

       
        /// <summary>
        /// 将string转Int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return Convert.ToInt32(str);
        }

    }
}
