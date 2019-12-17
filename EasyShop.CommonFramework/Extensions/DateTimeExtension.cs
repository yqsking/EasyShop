namespace System
{
    public  static class DateTimeExtension
    {
        /// <summary>
        /// 将当前时间转成Unix时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToUnixTimeSeconds(this DateTime time)
        {
            return new DateTimeOffset(time).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Unix时间戳转本地时间
        /// </summary>
        /// <param name="unixSecond"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this int unixSecond)
        {
           return DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc).AddSeconds(unixSecond).ToLocalTime();
        }
    }
}
