using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDockerApi.Cache
{
    public class CacheObject<T>
    {
        public CacheObject(T item, int cacheLifeTimeInSeconds)
        {
            this.Item = item;
            this.CreatedOn = DateTime.Now;
            this.LifeTimeInSeconds = cacheLifeTimeInSeconds;
            this.ExpiresOn = this.CreatedOn.AddSeconds(cacheLifeTimeInSeconds);
        }
        public T Item { get; set; }
        public int LifeTimeInSeconds  { get; set;  }
        public long MaxAgeInSeconds 
        {
            get
            {
                if (ExpiresOn > DateTime.Now)
                {
                    return (long)ExpiresOn.Subtract(DateTime.Now).TotalSeconds;
                }
                return 0;
            }
        }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
