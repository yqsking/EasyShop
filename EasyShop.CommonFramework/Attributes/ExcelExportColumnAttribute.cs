using System;

namespace EasyShop.CommonFramework.Attributes
{
    /// <summary>
    /// Excel导出特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public  class ExcelExportColumnAttribute:Attribute
    {
        /// <summary>
        /// Excel列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 默认列宽
        /// </summary>
        public int Width { get; set; } = 50;

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
        public ExcelExportColumnAttribute SetPropertyName(string propertyName)
        {
            PropertyName = propertyName;
            return this;
        }
    }
}
