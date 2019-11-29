using System;

namespace EasyShop.CommonFramework.Attributes
{
    /// <summary>
    /// Excel导出特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public  class ExcelColumnAttribute:Attribute
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 默认列宽
        /// </summary>
        public int Width { get; set; } = 100;

        /// <summary>
        /// 数据格式
        /// </summary>
        public string DataFormat { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 类[属性]
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ExcelColumnAttribute SetPropertyName(string propertyName)
        {
            PropertyName = propertyName;
            return this;
        }
    }
}
