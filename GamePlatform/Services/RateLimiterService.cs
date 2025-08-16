using GamePlatform.Services.Contracts;

namespace GamePlatform.Services;

public class RateLimiterService : IRateLimiterService
{
    private readonly IRedisService _redisService;

    public RateLimiterService(IRedisService redisService) => _redisService = redisService;

    public async Task<bool> IsAllowedAsync(
        string key, 
        int limit, 
        TimeSpan window)
    {
        var current = await _redisService.Db.StringIncrementAsync(key);
        if (current == 1) await _redisService.Db.KeyExpireAsync(key, window);
        return current <= limit;
    }
}
