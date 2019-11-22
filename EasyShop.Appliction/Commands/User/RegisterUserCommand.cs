using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using MediatR;

namespace EasyShop.Appliction.Commands.User
{
    /// <summary>
    /// 注册用户命令
    /// </summary>
    public  class RegisterUserCommand:IRequest<ApiResult<UserResponseDto>>
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
        /// 密码
        /// </summary>
        public string Password { get; set; }
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
    }
}
