using System.ComponentModel;

namespace EasyShop.CommonFramework.Const
{
    /// <summary>
    /// 用户类型常量
    /// </summary>
    public static  class UserTypeConst
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        public const string Manage = "Manage";

        /// <summary>
        /// 导购员
        /// </summary>
        [Description("导购员")]
        public const string ShoppingGuide = "ShoppingGuide";

        /// <summary>
        /// 购物者
        /// </summary>
        [Description("购物者")]
        public const string Shoppers = "Shoppers";
    }
}
