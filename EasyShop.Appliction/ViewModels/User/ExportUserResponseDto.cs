using EasyShop.CommonFramework.Attributes;
using EasyShop.CommonFramework.Const;

namespace EasyShop.Appliction.ViewModels.User
{
    /// <summary>
    /// 导出用户
    /// </summary>
    public  class ExportUserResponseDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [ExcelColumn(ColumnName ="用户名",DataType =ExcelDataTypeConst.String,Sort =1)]
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [ExcelColumn(ColumnName = "手机号", DataType = ExcelDataTypeConst.String, Sort = 2)]
        public string Phone { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [ExcelColumn(ColumnName = "用户头像", DataType = ExcelDataTypeConst.String, Sort = 3)]
        public string Photo { get; set; }
        /// <summary>
        /// qq号码
        /// </summary>
        [ExcelColumn(ColumnName = "qq号码", DataType = ExcelDataTypeConst.String, Sort = 4)]
        public string QQNumber { get; set; }
        /// <summary>
        /// 微信号码
        /// </summary>
        [ExcelColumn(ColumnName = "微信号码", DataType = ExcelDataTypeConst.String, Sort = 5)]
        public string WeCharNumber { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [ExcelColumn(ColumnName = "电子邮箱", DataType = ExcelDataTypeConst.String, Sort = 6)]
        public string Email { get; set; }
    }
}
