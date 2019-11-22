using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyShop.Dommain.Entitys.User
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Table("t_User_UserRole")]
    public  class UserRoleEntity:BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="roleId">角色id</param>
        public UserRoleEntity(string userId,string roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        /// <summary>
        /// 用户id
        /// </summary>
        [MaxLength(32),Required]
        public string UserId { get;private set; }

        /// <summary>
        /// 角色id
        /// </summary>
        [MaxLength(32), Required]
        public string RoleId { get; private set; }
    }
}
