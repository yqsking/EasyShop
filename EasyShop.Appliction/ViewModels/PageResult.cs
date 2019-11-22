using System.Collections.Generic;

namespace EasyShop.Appliction.ViewModels
{
    /// <summary>
    /// 通用分页查询返回结果
    /// </summary>
    /// <typeparam name="TResponseDto"></typeparam>
    public class PageResult<TResponseDto> : RequestResult where TResponseDto:BaseResponseDto
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 总页码
        /// </summary>
        public int TotalPageIndex { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalNumber { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<TResponseDto> Data { get; set; }
      
    }
}
