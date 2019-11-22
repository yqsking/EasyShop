using MediatR;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入命令
    /// </summary>
    public static  class RegisterCommandModule
    {
        /// <summary>
        /// 依赖注入命令
        /// </summary>
        /// <param name="collection"></param>
        public static void RegisterCommand(this IServiceCollection collection)
        {
            var commands = Assembly.Load("EasyShop.Appliction").GetTypes().Where(item => item.IsClass && item.Name.EndsWith("Command")).ToList();
            commands.ForEach(item=>collection.AddMediatR(item));
        }
    }
}
