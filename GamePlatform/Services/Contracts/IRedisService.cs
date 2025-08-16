using StackExchange.Redis;

namespace GamePlatform.Services.Contracts;

public interface IRedisService
{
    public IDatabase Db { get; }

    public ISubscriber Subscriber { get; }
}
