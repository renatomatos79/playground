using ServiceStack.Redis;
using System;

namespace CoreDockerApi.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly IRedisClientsManager redisClientsManager;
        public CacheManager(IRedisClientsManager redisClientsManager)
        {
            this.redisClientsManager = redisClientsManager;
        }
        private void AddKey(string key, string value, int expiresInSeconds)
        {
            using (var redis = redisClientsManager.GetClient())
            {
                redis.Set(key, value, DateTime.Now.AddSeconds(expiresInSeconds));
            }
        }
        private string GetKey(string key)
        {
            using (var redis = redisClientsManager.GetClient())
            {
                return redis.Get<string>(key);
            }
        }
        public void Add<T>(string key, T value, int expiresInSeconds)
        {
            var item = new CacheObject<T>(value, expiresInSeconds);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            AddKey(key, json, expiresInSeconds);
        }
        public CacheObject<T> Get<T>(string key)
        {
            var value = GetKey(key);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<CacheObject<T>>(value);
        }
    }
}