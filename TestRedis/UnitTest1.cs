using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Redis.Controllers;
using ServiceStack.Redis;

namespace TestRedis
{
	public class Tests
	{
		private IRedisClientsManager _clientsManager;
		
		[SetUp]
		public void Setup()
		{
			var redisReadWriteHosts = new List<string> {"localhost:6379"};
			var redisReadOnlyHosts = new List<string> {"localhost:6378"};
			_clientsManager = new PooledRedisClientManager(redisReadWriteHosts, redisReadOnlyHosts,
				new RedisClientManagerConfig
				{
					MaxReadPoolSize = 200,
					MaxWritePoolSize = 100,
				});
		}

		[Test]
		public async Task Test1()
		{
			var controller = new TestController(_clientsManager);
			var expected = controller.Create();
			var actual = await controller.Index();
			Assert.AreEqual(expected, actual);
		}
	}
}