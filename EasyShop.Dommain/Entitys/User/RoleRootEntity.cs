using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyShop.Dommain.Entitys.User
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [Table("t_User_RoleRoot")]
    public  class RoleRootEntity:BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRoleId">角色id</param>
        /// <param name="rootId">权限id</param>
        public RoleRootEntity(string userRoleId,string rootId)
        {
            UserRoleId = userRoleId;
            RootId = rootId;
        }

        /// <summary>
        /// 角色id
        /// </summary>
        [MaxLength(32), Required]
        public string UserRoleId { get;private set; }

        /// <summary>
        /// 权限id
        /// </summary>
        [MaxLength(32), Required]
        public string RootId { get; private set; }
    }
}
