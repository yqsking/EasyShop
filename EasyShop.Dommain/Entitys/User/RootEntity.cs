using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyShop.Dommain.Entitys.User
{
    /// <summary>
    /// 权限信息
    /// </summary>
    [Table("t_Table_Root")]
    public  class RootEntity:BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootName">权限名</param>
        /// <param name="rootCode">权限代码</param>
        /// <param name="remark">备注</param>
        public RootEntity(string rootName,string rootCode,string remark)
        {
            RootName = rootName;
            RootCode = rootCode;
            Remark = remark;
        }
        /// <summary>
        /// 权限名
        /// </summary>
        [MaxLength(50),Required]
        public string RootName { get; private set; }

        /// <summary>
        /// 设置权限名
        /// </summary>
        /// <param name="rootName"></param>
        public void SetRootName(string rootName)
        {
            RootName = rootName;
        }
        /// <summary>
        /// 权限代码
        /// </summary>
        [MaxLength(50),Required]
        public string RootCode { get; private set; }

        /// <summary>
        /// 设置权限代码
        /// </summary>
        /// <param name="rootCode"></param>
        public void SetRootCode(string rootCode)
        {
            RootCode = rootCode;
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
