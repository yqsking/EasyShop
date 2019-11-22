using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EasyShop.Dommain.Repositorys
{
    /// <summary>
    /// 仓储工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {

        /// <summary>
        /// 获取数据库上下文对象
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();


        /// <summary>
        /// 提交当前工作单元事务
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        /// 回滚当前工作单元事务
        /// </summary>
        /// <returns></returns>

        Task RollbackAsync();

    }
}
