using System;

namespace EasyShop.CommonFramework.Attributes
{
    /// <summary>
    /// Excel导入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public  class ExcelImportColumnAttribute: Attribute
    {
        public string ColumnName { get; set; }
    }
}
