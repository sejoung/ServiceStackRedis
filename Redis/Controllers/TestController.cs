using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Redis;

namespace Redis.Controllers
{
	public class TestController : Controller
	{
		private readonly IRedisClientsManager _redisClientsManager;

		private readonly string _key = "test";

		public TestController(IRedisClientsManager redisClientsManager)
		{
			_redisClientsManager = redisClientsManager;
		}

		public async Task<string> Index()
		{
			await using var client = await _redisClientsManager.GetReadOnlyClientAsync();
			return await client.GetValueAsync(_key);
		}


		public string Create()
		{
			using var master = _redisClientsManager.GetClient();
			var random = new Random().Next();
			master.Set(_key, random);
			return random.ToString();
		}
	}
}