namespace EasyShop.Appliction.DataTransferModels.User
{
    /// <summary>
    /// 查询多个用户搜索条件
    /// </summary>
    public  class GetUserPageListRequestDto:BaseSearchRequestDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// qq号
        /// </summary>
        public string QQNumber { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string WeCharNumber { get; set; }
    }
}
