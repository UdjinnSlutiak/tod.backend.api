using System;
namespace Tod.Services.Redis
{
	public class RedisConfiguration
	{
        public string InstanceName { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
    }
}

