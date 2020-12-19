namespace CoreDockerApi.Cache
{
    public interface ICacheManager
    {
        void Add<T>(string key, T value, int expiresInSeconds);
        CacheObject<T> Get<T>(string key);
    }
}
