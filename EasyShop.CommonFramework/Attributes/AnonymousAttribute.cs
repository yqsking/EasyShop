using System;

namespace EasyShop.CommonFramework.Attributes
{
    /// <summary>
    /// 匿名登录
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public  class AnonymousAttribute:Attribute
    {

    }
}
