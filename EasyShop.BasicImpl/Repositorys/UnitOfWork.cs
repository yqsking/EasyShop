using EasyShop.Dommain.Repositorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace EasyShop.BasicImpl.Repositorys
{
    /// <summary>
    /// 仓储工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// 数据库上下文对象
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// 单元事务
        /// </summary>
        private readonly IDbContextTransaction _dbContextTransaction ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContextTransaction = _dbContext.Database.BeginTransaction();
        }

        /// <summary>
        /// 获取数据库上下文对象
        /// </summary>
        /// <returns></returns>
        public  DbContext GetDbContext()
        {
            return _dbContext;
        }

        /// <summary>
        /// 提交当前工作单元事务
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            await _dbContextTransaction.CommitAsync();
            await _dbContextTransaction.DisposeAsync();
        }

        /// <summary>
        /// 回滚当前工作单元事务
        /// </summary>
        /// <returns></returns>
        public async Task RollbackAsync()
        {
            await _dbContextTransaction.RollbackAsync();
            await _dbContextTransaction.DisposeAsync();
        }

      
    }
}
