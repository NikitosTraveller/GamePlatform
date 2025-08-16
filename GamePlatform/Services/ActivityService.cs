using GamePlatform.Services.Contracts;

namespace GamePlatform.Services;

public class ActivityService : IActivityService
{
    private readonly IRedisService _redisService;
    private const string DailyLoginPrefix = "activity:daily:";
    private const string UniquePlayersKey = "activity:unique";

    public ActivityService(IRedisService redisService) => _redisService = redisService;

    // Track daily login using Bitmap
    public async Task MarkLoginAsync(string playerId)
    {
        var key = DailyLoginPrefix + DateTime.UtcNow.ToString("yyyyMMdd");
        if (!int.TryParse(playerId, out int offset)) offset = playerId.GetHashCode() & int.MaxValue;
        await _redisService.Db.StringSetBitAsync(key, offset, true);
    }

    public async Task<long> CountDailyActivePlayersAsync(DateTime date)
    {
        var key = DailyLoginPrefix + date.ToString("yyyyMMdd");
        return await _redisService.Db.StringBitCountAsync(key);
    }

    // Track unique players using HyperLogLog
    public async Task AddUniquePlayerAsync(string playerId)
    {
        await _redisService.Db.HyperLogLogAddAsync(UniquePlayersKey, playerId);
    }

    public async Task<long> GetUniquePlayerCountAsync()
    {
        return await _redisService.Db.HyperLogLogLengthAsync(UniquePlayersKey);
    }
}
