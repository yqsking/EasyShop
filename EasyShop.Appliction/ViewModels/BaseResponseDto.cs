using System;

namespace EasyShop.Appliction.ViewModels
{
    /// <summary>
    /// 响应dto基类
    /// </summary>
    public  class BaseResponseDto
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间字符串
        /// </summary>
        public string CreateTimeString => CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
