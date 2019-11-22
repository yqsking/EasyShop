using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// OrderByExpand排序拓展
    /// </summary>
    public static class OrderByExtension
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="expression">排序表达式</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, dynamic>> expression,bool isDesc) 
        {
            if(expression==null)
            {
                throw new Exception("排序表达式不能为空");
            }
            return isDesc ? source.OrderByDescending(expression) : source.OrderBy(expression);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="expression">排序表达式</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> OrderBy<TEntity>(this IEnumerable<TEntity> source, Expression<Func<TEntity, dynamic>> expression, bool isDesc)
        {
            if (expression == null)
            {
                throw new Exception("排序表达式不能为空");
            }
            return isDesc ? source.OrderByDescending(expression.Compile()): source.OrderBy(expression.Compile());
        }
    }
}
