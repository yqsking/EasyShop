namespace EasyShop.Appliction.ViewModels
{
    /// <summary>
    /// 请求信息
    /// </summary>
    public  class RequestResult
    {
        /// <summary>
        /// 请求状态
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 错误Id
        /// </summary>
        public string ErrorId { get; set; }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; }
    }
}
