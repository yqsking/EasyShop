using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入仓储
    /// </summary>
    public static class RegisterRepositorysModule
    {
        /// <summary>
        /// 依赖注入仓储
        /// </summary>
        /// <param name="collection"></param>
        public static void RegisterRepositorys(this IServiceCollection collection)
        {
            var iRepositorys = Assembly.Load("EasyShop.Dommain").GetTypes().Where(item => item.IsInterface && item.Name.EndsWith("Repository"));
            if (iRepositorys.Any())
            {
                var repositorys = Assembly.Load("EasyShop.BasicImpl").GetTypes().Where(item => item.IsClass && !item.IsAbstract && item.Name.EndsWith("Repository"));
                foreach(var item in iRepositorys )
                {
                    var impl = repositorys.FirstOrDefault(args=>item.IsAssignableFrom(args));
                    if(impl!=null)
                    {
                        collection.AddScoped(item,impl);
                    }
                }
            }

        }
    }
}
