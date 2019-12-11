using EasyShop.CommonFramework.Attributes;
using System.Collections.Generic;
using System.Reflection;

namespace System.Data
{
    /// <summary>
    /// DataTable拓展
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// DataTable转List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dataTable) where T:new()
        {
            if (dataTable.Rows.Count > 0)
            {
                List<T> list = new List<T>();
                PropertyInfo[] propertyInfos = typeof(T).GetProperties();
                foreach (DataRow row in dataTable.Rows)
                {
                    T model = new T();
                    foreach (var propertyInfo in propertyInfos)
                    {
                        string columnName= propertyInfo.GetCustomAttribute<ExcelImportColumnAttribute>()?.ColumnName??propertyInfo.Name;
                        var value = row[columnName];
                        if (value != null)
                        {
                            propertyInfo.SetValue(model, Convert.ChangeType(value, propertyInfo.PropertyType));
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
            else
            {
                return default;
            }

        }
    }
}
