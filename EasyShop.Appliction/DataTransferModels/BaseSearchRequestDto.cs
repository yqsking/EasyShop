﻿namespace EasyShop.Appliction.DataTransferModels
{
    /// <summary>
    /// 分页查询搜索条件基类
    /// </summary>
    public class BaseSearchRequestDto
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 10;

    }
}
