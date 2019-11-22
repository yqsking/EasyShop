using EasyShop.Dommain.Common;
using EasyShop.Dommain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EasyShop.Dommain.Repositorys
{
    /// <summary>
    /// 基础仓储接口
    /// </summary>
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// 添加一个或者多个实体模型
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddEntityAsync(params TEntity[] entity);

        /// <summary>
        /// 修改一个或者多个实体模型
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        Task UpdateEntityAsync(params TEntity[] entitys);

        /// <summary>
        /// 删除一个或者多个实体模型
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        Task DeleteEntityAsync(params TEntity[] entitys);

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns>记录数</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns>布尔值</returns>
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据主键查询单个实体模型
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        Task<TEntity> GetEntityAsync(string key);


        /// <summary>
        /// 根据条件查询单个实体模型
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns>实体</returns>
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据条件查询所有实体模型
        /// </summary>
        /// <param name="conditionExpression">条件表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isDesc">是否倒序(默认创建时间)</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetEntityAllListAsync(Expression<Func<TEntity, bool>> conditionExpression, Expression<Func<TEntity, dynamic>> orderByExpression, bool isDesc = true);

        /// <summary>
        /// 根据条件分页查询实体模型
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="conditionExpression">条件表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isDesc">是否倒序(默认创建时间)</param>
        /// <returns></returns>
        Task<PageResult<TEntity>> GetEntityPageList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> conditionExpression, Expression<Func<TEntity, dynamic>> orderByExpression, bool isDesc = true);


    }
}
