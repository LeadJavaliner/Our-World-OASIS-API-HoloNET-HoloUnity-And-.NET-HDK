using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ReferenceConsoleRedisApp
{
  class Program
  {
    static readonly String redisPassword = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
    static readonly String connectionString = "redis-15805.c90.us-east-1-3.ec2.cloud.redislabs.com:15805,password=" + redisPassword;
    static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
    static async Task Main(string[] args)
    {
      var db = redis.GetDatabase();
    }
  }
}