using System.Collections.Generic;

namespace EasyShop.CommonFramework.Results
{
    /// <summary>
    /// xml读取通用dto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public  class XmlLoadResult<T> where T:new ()
    {
        /// <summary>
        /// 当前xml节点
        /// </summary>
        public T Node { get; set; }

        /// <summary>
        /// 当前xml节点的所有子节点
        /// </summary>
        public IList<XmlLoadResult<T>> ChildNodes { get; set; }
    }
}
