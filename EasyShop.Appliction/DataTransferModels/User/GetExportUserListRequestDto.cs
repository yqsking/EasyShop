namespace EasyShop.Appliction.DataTransferModels.User
{
    /// <summary>
    /// 导出用户信息
    /// </summary>
    public  class GetExportUserListRequestDto
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
