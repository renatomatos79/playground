Hello dockers,

Thus far, we just build local images.
But at this specific moment, we are gonna do something different:
- Let's set up our docker locally in our host
- build and deliver our .NET Core APIs using Docker
- Create a remote repository using docker hub
- Finally, install our docker images using our playground home page 

Are you ready to docker?

1. As a Windows User, go to the Control Panel → Programs and Features → and enable Hyper-V
2. Check the options  Hyper-V Management Tools e Hyper-V Platform
3. Install Docker for Windows 
https://hub.docker.com/editions/community/docker-ce-desktop-windows/
4. Run and get authenticated into your Docker Desktop
5. Open your terminal (Command Prompt) and check if the docker is ok 

c:\>docker run hello-world

Hello from Docker!
This message shows that your installation appears to be working correctly.

====================================================================================

We are gonna build a similar project to the app in the link below:
https://github.com/renatomatos79/playground/tree/master/vs/CoreDockerApi

Follow the steps:

1. Open the Visual Studio 2019 and create a .Net Core Web Application
2. Name the project as CoreDockerApi
3. Specify the location as C:\Temp\docker-k8s\vs
4. Check the option "Keep the solution in the same project folder"
5. Click on the Create button
6. In the next window check the options: Empty project, .Net Core, and Asp Net Core 3.1
7. Uncheck Setup Https and Enable docker support (we are gonna do this manually)
8. Create a model class named ProductModel
	
	public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
	
9. Create a controller class named ProductController
	
	public class ProductController : Controller
    {
        [HttpGet("/products")]
        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            return await Task.FromResult(new List<ProductModel> 
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
                }
            });
        }
    }
	
10. In the startup class change the method ConfigureServices:

		public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }
		
11. Still in the startup class change the method Configure:	

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

12. Run your project and then using CURL invoke your route

C:\Users\Família>curl http://localhost:56879/products
[{"id":1,"name":"Eggs (box with 12 units)","price":1.5},{"id":2,"name":"Chocolate","price":1.99}]

====================================================================================

In the next step we are gonna build a Dockerfile wich will build and host our application

1. Create a Dockerfile using this path

https://raw.githubusercontent.com/renatomatos79/playground/master/vs/CoreDockerApi/Dockerfile

2. Building the Dockerfile image => docker build -t core-docker-api:1.0 .

C:\Temp\docker-k8s\vs\CoreDockerApi>docker build -t core-docker-api:1.0 .
[+] Building 0.9s (15/15) FINISHED
 => [internal] load build definition from Dockerfile                                                                                                                                                                                    0.1s
 => => transferring dockerfile: 32B                                                                                                                                                                                                     0.0s
 => [internal] load .dockerignore                                                                                                                                                                                                       0.0s
 => => transferring context: 35B                                                                                                                                                                                                        0.0s
 => [internal] load metadata for mcr.microsoft.com/dotnet/core/aspnet:3.1                                                                                                                                                               0.6s
 => [internal] load metadata for mcr.microsoft.com/dotnet/core/sdk:3.1                                                                                                                                                                  0.7s
 => [build-env 1/6] FROM mcr.microsoft.com/dotnet/core/sdk:3.1@sha256:3cb6a73ed35b6c4819156207a9b43d03898e6814dd58e4ae624334718a3746a8                                                                                                  0.0s
 => => resolve mcr.microsoft.com/dotnet/core/sdk:3.1@sha256:3cb6a73ed35b6c4819156207a9b43d03898e6814dd58e4ae624334718a3746a8                                                                                                            0.0s
 => [stage-1 1/3] FROM mcr.microsoft.com/dotnet/core/aspnet:3.1@sha256:f786e94436a4a4f8f86e3d86372d54d6fd39a4085c6416ecaa7096abb72139a0                                                                                                 0.0s
 => [internal] load build context                                                                                                                                                                                                       0.1s
 => => transferring context: 378B                                                                                                                                                                                                       0.0s
 => CACHED [stage-1 2/3] WORKDIR /app                                                                                                                                                                                                   0.0s
 => CACHED [build-env 2/6] WORKDIR /src                                                                                                                                                                                                 0.0s
 => CACHED [build-env 3/6] COPY [CoreDockerApi.csproj, .]                                                                                                                                                                               0.0s
 => CACHED [build-env 4/6] RUN dotnet restore CoreDockerApi.csproj                                                                                                                                                                      0.0s
 => CACHED [build-env 5/6] COPY . .                                                                                                                                                                                                     0.0s
 => CACHED [build-env 6/6] RUN dotnet build "CoreDockerApi.csproj" -c Release -o /app/build                                                                                                                                             0.0s
 => CACHED [stage-1 3/3] COPY --from=build-env /app/build .                                                                                                                                                                             0.0s
 => exporting to image                                                                                                                                                                                                                  0.0s
 => => exporting layers                                                                                                                                                                                                                 0.0s
 => => writing image sha256:82953195964293d6e44dc4a84bdc0d066dfe7eb61ac076ef870177dd097765d7                                                                                                                                            0.0s
 => => naming to docker.io/library/core-docker-api:1.0
 
3. Running the container exposing the internal port 80 over the external port 8001

C:\Temp\docker-k8s\vs\CoreDockerApi>docker run -d -p 8001:80 --name core_docker_api core-docker-api:1.0
4eaadcc2e40afa9a129372e52293d631bb79436c716421d41584f09f068668af

C:\Temp\docker-k8s\vs\CoreDockerApi>docker ps
CONTAINER ID   IMAGE                 COMMAND                  CREATED          STATUS          PORTS                  NAMES
4eaadcc2e40a   core-docker-api:1.0   "dotnet CoreDockerAp…"   18 seconds ago   Up 17 seconds   0.0.0.0:8001->80/tcp   core_docker_api

4. Finally, let's take a look at the API result

C:\Temp\docker-k8s\vs\CoreDockerApi>curl http://localhost:8001/products
[{"id":1,"name":"Eggs (box with 12 units)","price":1.5},{"id":2,"name":"Chocolate","price":1.99}]