using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceStack.Redis;

namespace Redis
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			var redisReadWriteHosts = new List<string> {"redisprimary:6379"};
			var redisReadOnlyHosts = new List<string> {"redissecondary:6379"};
			var pooledRedisClientManager = new PooledRedisClientManager(redisReadWriteHosts, redisReadOnlyHosts,
				new RedisClientManagerConfig
				{
					MaxReadPoolSize = 200,
					MaxWritePoolSize = 100,
				})
			{
				PoolTimeout = 10,
			};
			services.AddControllers();
			services.AddSingleton(typeof(IRedisClientsManager), pooledRedisClientManager);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Test}/{action=Index}");
			});
		}
	}
}