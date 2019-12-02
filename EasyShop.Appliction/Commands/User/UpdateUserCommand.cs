using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using MediatR;

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
        public string UserName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Photo { get; set; }
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
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
    }
}
