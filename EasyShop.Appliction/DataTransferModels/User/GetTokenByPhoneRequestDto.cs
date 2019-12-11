using System.ComponentModel.DataAnnotations;

namespace EasyShop.Appliction.DataTransferModels.User
{
    /// <summary>
    /// 根据手机号登录
    /// </summary>
    public  class GetTokenByPhoneRequestDto
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage ="手机号码不能为空")]
        public string Phone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage ="密码不能为空")]
        public string Password { get; set; }
    }
}
