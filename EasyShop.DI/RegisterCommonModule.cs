using EasyShop.BasicImpl.DBContext;
using EasyShop.BasicImpl.Repositorys;
using EasyShop.Dommain.Repositorys;
using Microsoft.EntityFrameworkCore;


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入公共服务
    /// </summary>
    public static class RegisterCommonModule
    {
        /// <summary>
        /// 依赖注入公共服务
        /// </summary>
        /// <param name="collection"></param>
        public static void RegisterCommon(this IServiceCollection collection)
        {
            collection.AddScoped(typeof(DbContext),typeof(EasyShopDBContext));
            collection.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));

        }
    }
}
