using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EasyShop.CommonFramework.Helpers
{
    /// <summary>
    /// 缓存工具类
    /// </summary>
    public class CacheHelper
    {
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        public CacheHelper(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 根据Key获取字符串缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetstringAsync(string key)
        {
            return await _cache.GetStringAsync(key);
        }

        /// <summary>
        /// 根据Key获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetObjectAsync<T>(string key) where T : new()
        {
            byte[] value = await _cache.GetAsync(key);
            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value));
            }
            else
            {
                return default;
            }

        }

        /// <summary>
        /// 根据Key缓存字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public async Task SetstringAsync(string key,string value,int minutes=30)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(minutes);
            await  _cache.SetStringAsync(key,value,options);
        }

        /// <summary>
        /// 根据Key缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t">被缓存的对象</param>
        /// <param name="minutes">缓存周期，默认30分钟</param>
        /// <returns></returns>
        public async Task SetObjectAsync<T>(string key, T t, int minutes = 30) where T : new()
        {
            if (t != null)
            {
                byte[] value = null;
                value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(minutes);
                await _cache.SetAsync(key, value, options);
            }

        }

        /// <summary>
        /// 根据Key移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

    }
}
