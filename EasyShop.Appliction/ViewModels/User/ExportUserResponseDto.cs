﻿using EasyShop.CommonFramework.Attributes;
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
        [ExcelExportColumn(ColumnName ="用户名",Width =100,DataType =ExcelDataTypeConst.String,Sort =1)]
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [ExcelExportColumn(ColumnName = "手机号",Width =100, DataType = ExcelDataTypeConst.String, Sort = 2)]
        public string Phone { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [ExcelExportColumn(ColumnName = "用户头像",Width =50, DataType = ExcelDataTypeConst.Photo, Sort = 3)]
        public string Photo { get; set; }
        /// <summary>
        /// qq号码
        /// </summary>
        [ExcelExportColumn(ColumnName = "qq号码",Width =100, DataType = ExcelDataTypeConst.String, Sort = 4)]
        public string QQNumber { get; set; }
        /// <summary>
        /// 微信号码
        /// </summary>
        [ExcelExportColumn(ColumnName = "微信号码",Width =100, DataType = ExcelDataTypeConst.String, Sort = 5)]
        public string WeCharNumber { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [ExcelExportColumn(ColumnName = "电子邮箱",Width =100, DataType = ExcelDataTypeConst.String, Sort = 6)]
        public string Email { get; set; }
    }
}
