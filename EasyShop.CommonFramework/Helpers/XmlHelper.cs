using EasyShop.CommonFramework.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace BasicFramework.Common.Helpers
{
    /// <summary>
    /// xml工具类
    /// </summary>
    public static  class XmlHelper
    {
        /// <summary>
        /// 加载xml文件第一个父节点
        /// </summary>
        /// <param name="filePathUrl">xml文件路径(</param>
        /// <returns></returns>
        public static XElement LoadXmlFirstElement(string filePathUrl)
        {
            if(!System.IO.File.Exists(filePathUrl))
            {
                throw new Exception($"没有找到指定路径文件：{filePathUrl}");
            }
            XDocument xDocument = XDocument.Load(filePathUrl);
            return xDocument.Elements().FirstOrDefault();
        }

        /// <summary>
        /// 获取指定xml节点下面所有子节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xElement"></param>
        /// <returns></returns>
        public static IList<XmlLoadResult<T>> GetAllChildElement<T>(XElement xElement) where T:new()
        {
            List<XmlLoadResult<T>> list = new List<XmlLoadResult<T>>();
            if(xElement.Elements().Any())
            {
               PropertyInfo[] propertyInfos= typeof(T).GetProperties();
               foreach (var element in xElement.Elements())
                {
                    XmlLoadResult<T> result = new XmlLoadResult<T>();
                    T node = new T();
                    foreach (var propertyinfo in propertyInfos)
                    {
                        var value = element.Attribute(propertyinfo.Name)?.Value;
                        if(value!=null)
                        {
                            propertyinfo.SetValue(node,Convert.ChangeType(value,propertyinfo.PropertyType));
                        }
                    }
                    result.Node = node;
                    if(element .Elements().Any())
                    {
                       result.ChildNodes= GetAllChildElement<T>(element);
                    }
                    list.Add(result);
                }
            }
            return list;
        }
    }
}
