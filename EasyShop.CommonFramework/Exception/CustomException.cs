namespace EasyShop.CommonFramework.Exception
{
    /// <summary>
    /// 自定义异常(自定义异常不记录错误日志)
    /// </summary>
    public class CustomException : System.Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage">异常信息</param>
        public CustomException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
