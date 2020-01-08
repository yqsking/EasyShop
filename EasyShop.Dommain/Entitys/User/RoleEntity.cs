using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyShop.Dommain.Entitys.User
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [Table("t_user_role")]
    public  class RoleEntity:BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="roleCode">角色代码</param>
        /// <param name="remark">备注</param>
        public RoleEntity(string roleName,string roleCode,string remark)
        {
            RoleName = roleName;
            RoleCode = roleCode;
            Remark = remark;
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        [MaxLength(50), Required]
        public string RoleName { get; private set; }

        /// <summary>
        /// 设置角色名称
        /// </summary>
        /// <param name="roleName"></param>
        public void SetRoleName(string roleName)
        {
            RoleCode = roleName;
        }

        /// <summary>
        /// 角色代码
        /// </summary>
        [MaxLength(50), Required]
        public string RoleCode { get; private set; }

        /// <summary>
        /// 设置角色代码
        /// </summary>
        /// <param name="roleCode"></param>
        public void SetRoleCode(string roleCode)
        {
            RoleCode = roleCode;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; private set; }

        /// <summary>
        /// 设置备注
        /// </summary>
        /// <param name="remark"></param>
        public void SetRemark(string remark)
        {
            Remark = remark;
        }
    }
}
