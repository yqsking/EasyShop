using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using MediatR;
using System.ComponentModel.DataAnnotations;

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
        [MaxLength(50,ErrorMessage ="用户名长度超过上限")]
        [Required(ErrorMessage ="用户名不能为空")]
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(20,ErrorMessage ="手机号长度超过上限")]
        [Required(ErrorMessage ="手机号不能为空")]
        [Phone(ErrorMessage ="手机号格式错误")]
        public string Phone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(10,ErrorMessage ="密码长度超过上限")]
        [Required(ErrorMessage ="密码不能为空")]
        public string Password { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [MaxLength(200,ErrorMessage ="用户头像地址长度超过上限")]
        public string Photo { get; set; }
        /// <summary>
        /// qq号码
        /// </summary>
        [MaxLength(50,ErrorMessage ="qq号码长度超过上限")]
        public string QQNumber { get; set; }
        /// <summary>
        /// 微信号码
        /// </summary>
        [MaxLength(50,ErrorMessage ="微信号码长度超过上限")]
        public string WeCharNumber { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [MaxLength(50,ErrorMessage ="电子邮箱长度超过上限")]
        public string Email { get; set; }
    }
}
