using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// WhereIf筛选拓展方法
    /// </summary>
    public static class WhereIfExtension
    {
        /// <summary>
        /// 当前置条件成立时，返回符合筛选条件的数据源;不成立时，返回所有数据源
        /// </summary>
        /// <typeparam name="TEntity">实体模型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="condition">前置条件</param>
        /// <param name="expression">筛选条件</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> WhereIf<TEntity>(this IEnumerable<TEntity> source,bool condition,Expression<Func<TEntity,bool>> expression)
        {
            return condition ? source.Where(expression.Compile()) : source;
        }

        /// <summary>
        /// 当前置条件成立时，返回符合筛选条件的数据源;不成立时，返回所有数据源
        /// </summary>
        /// <typeparam name="TEntity">实体模型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="condition">前置条件</param>
        /// <param name="expression">筛选条件</param>
        /// <returns></returns>
        public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> source, bool condition, Expression<Func<TEntity, bool>> expression)
        {
            return condition ? source.Where(expression):source;
        }
    }
}
