using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace EasyShop.CommonFramework.Helpers
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static  class ConstHelper
    {
        /// <summary>
        /// 获取常量所有描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string,string> GetAllDescription<T>() where T:new()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in typeof(T).GetFields())
            {
                string value = item.GetCustomAttribute<DescriptionAttribute>()?.Description;
                dic.Add(item.GetValue(null).ToString(),value);
            }
            return dic;
        }
    }
}
