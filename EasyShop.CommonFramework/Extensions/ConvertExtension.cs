using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using EasyShop.CommonFramework.Exception;
using Newtonsoft.Json;

namespace  EasyShop.CommonFramework.Expands
{
    /// <summary>
    /// 类型转换拓展
    /// </summary>
    public static  class ConvertExtension
    {
        /// <summary>
        /// 字符串转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToModel<T>(this string str) where T:new()
        {
            if(string.IsNullOrWhiteSpace(str))
            {
                throw new CustomException("当前字符串值为空");
            }
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 实体转字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static string ToJosn<T>(this T model) where T:new()
        {
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this DataTable dataTable) where T:new()
        {
            List<T> list = new List<T>();
            PropertyInfo[] propertyInfos= typeof(T).GetProperties();
            if(dataTable.Rows.Count>0)
            {
                foreach(DataRow row in dataTable.Rows)
                {
                    T model = new T();
                    foreach(var propertyInfo in propertyInfos)
                    {
                        var value = row[propertyInfo.Name];
                        if (value != null)
                        {
                            propertyInfo.SetValue(model, Convert.ChangeType(value, propertyInfo.PropertyType));
                        }
                    }
                    list.Add(model);
                }
            }
            return list;

        }

        /// <summary>
        /// IList转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> list) where T:new()
        {
            DataTable dt = new DataTable(typeof(T).Name);
            if(list.Any())
            {
                PropertyInfo[] propertyInfos= typeof(T).GetProperties();
                dt.Columns.AddRange(propertyInfos.Select(item=>new DataColumn(item.Name,item.PropertyType)).ToArray());
                foreach(var item in list)
                {
                    var rows= propertyInfos.Select(args => args.GetValue(item)).ToArray();
                    dt.Rows.Add(rows);
                }
            }
            return dt;
        }
    }
}
