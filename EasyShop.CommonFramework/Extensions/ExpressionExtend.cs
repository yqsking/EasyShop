namespace System.Linq.Expressions
{
    /// <summary>
    /// 表达式目录树拓展
    /// </summary>
    public static class ExpressionExtend
    {
        /// <summary>
        /// 合并表达式(And)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="leftExpression"></param>
        /// <param name="rightExpression"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> And<TEntity>(this Expression<Func<TEntity, bool>> leftExpression, Expression<Func<TEntity, bool>> rightExpression)
        {
            ParameterExpression newParameter = Expression.Parameter(typeof(TEntity), "parameter");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);

            var left = visitor.Replace(leftExpression.Body);
            var right = visitor.Replace(rightExpression.Body);
            var body = Expression.And(left, right);
            return Expression.Lambda<Func<TEntity, bool>>(body, newParameter);

        }
        /// <summary>
        /// 合并表达式(Or)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="leftExpression"></param>
        /// <param name="rightExpression"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> Or<TEntity>(this Expression<Func<TEntity, bool>> leftExpression, Expression<Func<TEntity, bool>> rightExpression)
        {

            ParameterExpression newParameter = Expression.Parameter(typeof(TEntity), "parameter");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);

            var left = visitor.Replace(leftExpression.Body);
            var right = visitor.Replace(rightExpression.Body);
            var body = Expression.Or(left, right);
            return Expression.Lambda<Func<TEntity, bool>>(body, newParameter);
        }
    }


    /// <summary>
    /// 建立新表达式
    /// </summary>
    internal class NewExpressionVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _newParameter;
        public NewExpressionVisitor(ParameterExpression param)
        {
            _newParameter = param;
        }
        public Expression Replace(Expression exp)
        {
            return Visit(exp);
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _newParameter;
        }
    }
}
