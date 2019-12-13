using EasyShop.Appliction.ViewModels;

namespace EasyShop.Appliction.ViewModels.User
{
    /// <summary>
    /// 用户基础信息
    /// </summary>
    public class UserResponseDto : BaseResponseDto
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
        /// 用户头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// qq号码
        /// </summary>
        public string QQNumber { get; set; }
        /// <summary>
        /// 微信号码
        /// </summary>
        public string WeCharNumber { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType { get;  set; }

      
        /// <summary>
        /// 用户状态
        /// </summary>
        public string UserState { get;  set; }


    }
}
