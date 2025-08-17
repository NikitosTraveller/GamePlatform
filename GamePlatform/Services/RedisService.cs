using GamePlatform.Services.Contracts;
using StackExchange.Redis;

namespace GamePlatform.Services;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer _redis;

    public RedisService(IConfiguration config)
    {
        var connection = config["Redis:Connection"] ?? "redis:6379";
        var options = ConfigurationOptions.Parse(connection);
        options.AbortOnConnectFail = false;
        options.ConnectRetry = 5;
        options.ConnectTimeout = 5000;
        _redis = ConnectionMultiplexer.Connect(options);
    }

    public IDatabase Db => _redis.GetDatabase();

    public ISubscriber Subscriber => _redis.GetSubscriber();
}
