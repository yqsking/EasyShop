using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EasyShop.Appliction.Commands.User
{
    /// <summary>
    /// 更新用户基本资料命令
    /// </summary>
    public  class UpdateUserCommand:IRequest<ApiResult<UserResponseDto>>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// 设置用户id
        /// </summary>
        /// <param name="id"></param>
        public void SetId(string id)
        {
            Id = id;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(50,ErrorMessage ="用户名长度超过上限(50字符)"),Required(ErrorMessage ="用户名不能为空")]
        public string UserName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [MaxLength(200,ErrorMessage ="用户头像地址长度超过上限(200字符)"), Required(ErrorMessage = "用户名不能为空")]
        public string Photo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(20,ErrorMessage ="手机号长度超过上限(20字符)"),Required(ErrorMessage ="手机号不能为空")]
        public string Phone { get; set; }
        /// <summary>
        /// qq号
        /// </summary>
        [MaxLength(50,ErrorMessage ="qq号长度超过上限(50字符)")]
        public string QQNumber { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        [MaxLength(50, ErrorMessage = "微信号长度超过上限(50字符)")]
        public string WeCharNumber { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [MaxLength(50, ErrorMessage = "电子邮箱长度超过上限(50字符)")]
        public string Email { get; set; }
    }
}
