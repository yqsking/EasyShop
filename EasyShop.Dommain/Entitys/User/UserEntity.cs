using EasyShop.CommonFramework.Const;
using EasyShop.CommonFramework.Exception;
using EasyShop.CommonFramework.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyShop.Dommain.Entitys.User
{
    /// <summary>
    /// 用户基础信息
    /// </summary>
    [Table("t_User_User")]
    public  class UserEntity:BaseEntity
    {

        private UserEntity() { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="phone">手机号</param>
        /// <param name="password">密码</param>
        /// <param name="photo">用户头像</param>
        /// <param name="qqNumber">qq号码</param>
        /// <param name="weCharNumber">微信号</param>
        /// <param name="email">邮箱</param>
        /// <param name="userType">用户类型</param>
        public UserEntity(string userName, string phone, string password, string photo,string qqNumber,string weCharNumber,string email,string userType)
        {
            UserName = userName;
            Phone = phone;
            Password = password;
            Photo = photo;
            QQNumber = qqNumber;
            WeCharNumber = weCharNumber;
            Email = email;
            UserType = userType;
            UserState = UserStateConst.Normal;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(50),Required]
        public string UserName { get; private set; }

        /// <summary>
        /// 设置用户名
        /// </summary>
        /// <param name="userName"></param>
        public void SetUserName(string userName)
        {
            UserName = userName;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(20),Required]
        public string Phone { get; private set; }

        /// <summary>
        /// 设置手机号
        /// </summary>
        /// <param name="phone"></param>
        public void SetPhone(string phone)
        {
            Phone = phone;
        }

        /// <summary>
        /// 登录密码
        /// </summary>
        [MaxLength(10), Required]
        public string Password { get; private set; }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password)
        {
            Password = password;
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        [MaxLength(200)]
        public string Photo { get; private set; }

        /// <summary>
        /// 设置用户头像
        /// </summary>
        /// <param name="photo"></param>
        public void SetPhoto(string photo)
        {
            Photo = photo;
        }

        /// <summary>
        /// qq号码
        /// </summary>
        [MaxLength(50)]
        public string QQNumber { get; private set; }
        /// <summary>
        /// 设置qq号码
        /// </summary>
        /// <param name="qqNumber"></param>
        public void SetQQNumber(string qqNumber)
        {
            QQNumber = qqNumber;
        }
        /// <summary>
        /// 微信号码
        /// </summary>
        [MaxLength(50)]
        public string WeCharNumber { get;private set; }

        /// <summary>
        /// 设置微信号码
        /// </summary>
        /// <param name="weCharNumber"></param>
        public void SetWeCharNumber(string weCharNumber)
        {
            WeCharNumber = weCharNumber;
        }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [MaxLength(50)]
        public string Email { get;private set; }

        /// <summary>
        /// 设置电子邮箱
        /// </summary>
        /// <param name="email"></param>
        public void SetEmail(string email)
        {
            Email = email;
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        [MaxLength(50),Required]
        public string UserType { get; private set; }

        /// <summary>
        /// 设置用户类型
        /// </summary>
        /// <param name="userType"></param>
        public void SetUserType(string userType)
        {
            string root = ConstHelper.GetAllDescription<UserTypeConst>()[UserTypeConst.Manage];
            if (UserType == UserTypeConst.Manage)
            {
                throw new CustomException($"{root}不能更改用户状态！");
            }
            UserType = userType;
        }

        /// <summary>
        /// 用户状态
        /// </summary>
        [MaxLength(50),Required]
        public string UserState { get; private set; }

        /// <summary>
        /// 设置用户状态
        /// </summary>
        /// <param name="userState"></param>
        public void SetUserState(string userState)
        {
            string root= ConstHelper.GetAllDescription<UserTypeConst>()[UserTypeConst.Manage];
            if(UserType==UserTypeConst.Manage)
            {
                throw new CustomException($"{root}不能更改用户状态！");
            }
            UserState = userState;
        }
      
    }
}
