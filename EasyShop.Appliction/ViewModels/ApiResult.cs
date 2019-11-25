namespace EasyShop.Appliction.ViewModels
{
    /// <summary>
    /// 通用请求结果
    /// </summary>
    public  class ApiResult<TResponseDto>:RequestResult 
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        public TResponseDto Data { get; set; }
    }
}
