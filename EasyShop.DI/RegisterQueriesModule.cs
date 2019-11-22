using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入查询器
    /// </summary>
    public static class RegisterQueriesModule
    {
        /// <summary>
        /// 依赖注入查询器
        /// </summary>
        /// <param name="collection"></param>
        public static void RegisterQueries(this IServiceCollection collection)
        {
            var iQueries = Assembly.Load("EasyShop.Appliction").GetTypes().Where(item => item.IsInterface && item.Name.EndsWith("Queries")).ToList();
            if (iQueries.Any())
            {
                var queries = Assembly.Load("EasyShop.Appliction").GetTypes().Where(item=>item.IsClass&&!item.IsAbstract && item.Name.EndsWith("Queries")).ToList();
                foreach(var item in iQueries)
                {
                    var impl = queries.FirstOrDefault(args=>item.IsAssignableFrom(args));
                    if(impl!=null)
                    {
                        collection.AddScoped(item,impl);
                    }
                }
            }
        }
    }
}
