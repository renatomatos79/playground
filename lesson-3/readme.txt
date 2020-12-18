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

