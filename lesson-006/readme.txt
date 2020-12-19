Hello dockers,

In lesson 005 we learned how to start and play with redis.

At this moment I just intend to show you how to update our .Net Core API 
in order to prepare it to support redis cache

Are you ready?

Attention!
The whole solution is available at 
https://github.com/renatomatos79/playground/tree/master/vs/CoreDockerApiWithRedis

1. Let's start using the previous solution (withoud redis)
2. Clone the repository https://github.com/renatomatos79/playground.git 
3. Open the project vs/CoreDockerApi (this project was built without redis) and add the following nugets

	Newtonsoft.Json Version="12.0.3"
    ServiceStack.Redis Version="5.10.2"
	
4. Add to this project a folder named Cache
5. In this Cache folder, create a class CacheObject 

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

6. Add an interface to manager our cache 

	public interface ICacheManager
    {
        void Add<T>(string key, T value, int expiresInSeconds);
        CacheObject<T> Get<T>(string key);
    }
	
7. Not, let's implement our interface
	
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
	
8. 	In the Startup class change the ConfigureService method

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers();
		services.AddHttpContextAccessor();
		services.AddSingleton<ICacheManager>(sp =>
		{
			// our Redis connection will be established using an Environment variable REDIS_HOST
			var host = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "redisserver:6379";
			var redisClient = new RedisManagerPool(host);
			return new CacheManager(redisClient);
		});
	}
		
9. 	Go to the ProductController class and inject the interface IHttpContextAccessor

	public ProductController(ICacheManager cacheManager, IHttpContextAccessor httpContextAccessor)
	{
		this.cacheManager = cacheManager;
		this.httpContextAccessor = httpContextAccessor;
	}
	
10. Change the GetProductAsync method
	
	[HttpGet("/products")]
	public async Task<IActionResult> GetProductsAsync()
	{
		var cache = cacheManager.Get<List<ProductModel>>("products");
		if (cache != null && cache.Item != null)
		{
			var headers = this.httpContextAccessor.HttpContext.Response.GetTypedHeaders();
			headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
			{
				Public = true,
				MaxAge = TimeSpan.FromSeconds(cache.MaxAgeInSeconds)
			};
			return await Task.FromResult(Ok(cache.Item));
		}
		var result = new List<ProductModel>
		{
			new ProductModel
			{
				Id = 1,
				Name = "Eggs (box with 12 units)",
				Price = (decimal)1.50
			},
			new ProductModel
			{
				Id = 2,
				Name = "Chocolate",
				Price = (decimal)1.99
			},
			new ProductModel
			{
				Id = 3,
				Name = "Butter (President)",
				Price = (decimal)2.25
			},
			new ProductModel
			{
				Id = 4,
				Name = "Codfish (Pascoal)",
				Price = (decimal)12.25
			},
			new ProductModel
			{
				Id = 5,
				Name = "Cheese 500g (Flamingo)",
				Price = (decimal)2.40
			},
			new ProductModel
			{
				Id = 6,
				Name = "Yogurt Danone (6 units)",
				Price = (decimal)1.45
			},
			new ProductModel
			{
				Id = 7,
				Name = "Bread (6 units)",
				Price = (decimal)0.45
			}
		};
		return await Task.FromResult(Ok(result));
	}	
	
11. Finally, let's create a temporary method to add itens to the redis cache 

	[HttpPut("/products/{cache}")]
	public async Task<IActionResult> PutProductsAsync([FromRoute] string cache, [FromBody] List<ProductModel> products)
	{
		this.cacheManager.Add(cache, products, 30);
		return await Task.FromResult(NoContent());
	}	
	
12. We are almost there! Let's rebuild and increment our Docker image to version 1.1

	cd ...\vs\CoreDockerApiWithRedis	
	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>docker build -t core-docker-api:1.1 .
	[+] Building 1.1s (15/15) FINISHED
	 => [internal] load build definition from Dockerfile                                                                                                                                       0.0s
	 => => transferring dockerfile: 32B                                                                                                                                                        0.0s
	 => [internal] load .dockerignore                                                                                                                                                          0.0s
	 => => transferring context: 35B                                                                                                                                                           0.0s
	 => [internal] load metadata for mcr.microsoft.com/dotnet/core/aspnet:3.1                                                                                                                  0.8s
	 => [internal] load metadata for mcr.microsoft.com/dotnet/core/sdk:3.1                                                                                                                     0.8s
	 => [stage-1 1/3] FROM mcr.microsoft.com/dotnet/core/aspnet:3.1@sha256:f786e94436a4a4f8f86e3d86372d54d6fd39a4085c6416ecaa7096abb72139a0                                                    0.0s
	 => [build-env 1/6] FROM mcr.microsoft.com/dotnet/core/sdk:3.1@sha256:3cb6a73ed35b6c4819156207a9b43d03898e6814dd58e4ae624334718a3746a8                                                     0.0s
	 => => resolve mcr.microsoft.com/dotnet/core/sdk:3.1@sha256:3cb6a73ed35b6c4819156207a9b43d03898e6814dd58e4ae624334718a3746a8                                                               0.0s
	 => [internal] load build context                                                                                                                                                          0.0s
	 => => transferring context: 526B                                                                                                                                                          0.0s
	 => CACHED [stage-1 2/3] WORKDIR /app                                                                                                                                                      0.0s
	 => CACHED [build-env 2/6] WORKDIR /src                                                                                                                                                    0.0s
	 => CACHED [build-env 3/6] COPY [CoreDockerApi.csproj, .]                                                                                                                                  0.0s
	 => CACHED [build-env 4/6] RUN dotnet restore CoreDockerApi.csproj                                                                                                                         0.0s
	 => CACHED [build-env 5/6] COPY . .                                                                                                                                                        0.0s
	 => CACHED [build-env 6/6] RUN dotnet build "CoreDockerApi.csproj" -c Release -o /app/build                                                                                                0.0s
	 => CACHED [stage-1 3/3] COPY --from=build-env /app/build .                                                                                                                                0.0s
	 => exporting to image                                                                                                                                                                     0.0s
	 => => exporting layers                                                                                                                                                                    0.0s
	 => => writing image sha256:8ed861d7f8738baeca401858e8652450de59e5e19d957bafb0f0536a0e74e9ef                                                                                               0.0s
	 => => naming to docker.io/library/core-docker-api:1.1
	
13. Remove a possible and preivous container instance core_docker_api	
	
	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>docker container stop core_docker_api
	core_docker_api
	
	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>docker container rm core_docker_api
	core_docker_api
	
14. Take the redisserver IP 

	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>docker container inspect redisserver | grep IPAddress
    "SecondaryIPAddresses": null,
    "IPAddress": "172.17.0.6",
    "IPAddress": "172.17.0.6",
	
	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>docker ps | grep redisserver
	9f163d588adf   redis     "docker-entrypoint.s…"   10 hours ago   Up 10 hours   0.0.0.0:6379->6379/tcp   redisserver
	
	Ok! We already have the IP 172.17.0.6 and Port 6379
	
	Let's check if this IP Address is reachable! Run the alpine instance in order to ping the internal RedisServer IP Address
	
	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>docker run -d -it --name myalpine alpine
	6f7caa2175e3e7ee087afc8cfdcb387a7b77e6d3aa96419c6786081c570aba9a
	
	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>docker container start -ai myalpine
	/ #
	
	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>docker container start -ai myalpine
	/ # ping 172.17.0.6
	PING 172.17.0.6 (172.17.0.6): 56 data bytes
	64 bytes from 172.17.0.6: seq=0 ttl=64 time=0.160 ms
	64 bytes from 172.17.0.6: seq=1 ttl=64 time=0.214 ms
	64 bytes from 172.17.0.6: seq=2 ttl=64 time=0.213 ms
	
	Ok! Great! :)
	
	
15. Run a new instance of our docker image core-docker-api:1.1 

	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>docker run -d -p 8001:80 --name core_docker_api -e REDIS_HOST=172.17.0.6:6379 core-docker-api:1.1
	19b29449d2347cde123a80cb5d2e16f7c11b02196449f453b1bd299b84e745d5
	
16. Let's play!

	Request the products route to see the whole list
	
	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>curl http://localhost:8001/products
	[{"id":1,"name":"Eggs (box with 12 units)","price":1.5},{"id":2,"name":"Chocolate","price":1.99},{"id":3,"name":"Butter (President)","price":2.25},{"id":4,"name":"Codfish (Pascoal)","price":12.25},{"id":5,"name":"Cheese 500g (Flamingo)","price":2.4},{"id":6,"name":"Yogurt Danone (6 units)","price":1.45},....
	
	Let's update our "products" cache key
	
	curl -X PUT http://localhost:8001/products/products -H "content-type: application/json" -d "[{\"Id\":1,\"Name\":\"From cache\",\"Price\":1.5}]"
	
	Let's confirm the list size has decreased according to the items we had sent to the cache 
	
	C:\Temp\docker-k8s\vs\CoreDockerApiWithRedis>curl http://localhost:8001/products
	[{"id":1,"name":"From cache","price":1.5}]
	
	
	