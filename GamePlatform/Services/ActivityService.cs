using GamePlatform.Services.Contracts;

namespace GamePlatform.Services;

public class ActivityService : IActivityService
{
    private readonly IRedisService _redisService;
    private readonly IPlayerService _playerService;
    private const string DailyLoginPrefix = "activity:daily:";
    private const string UniquePlayersKey = "activity:unique";

    public ActivityService(
        IRedisService redisService,
        IPlayerService playerService)
    {
        _redisService = redisService;
        _playerService = playerService;
    }

    public async Task MarkLoginAsync(string playerId)
    {
        var player = await _playerService.GetPlayerAsync(playerId);

        if (player == null) {
            return;
        }

        var key = DailyLoginPrefix + DateTime.UtcNow.ToString("yyyyMMdd");
        if (!int.TryParse(playerId, out int offset)) offset = playerId.GetHashCode() & int.MaxValue;
        await _redisService.Db.StringSetBitAsync(key, offset, true);
    }

    public async Task<long> CountDailyActivePlayersAsync(DateTime date)
    {
        var key = DailyLoginPrefix + date.ToString("yyyyMMdd");
        return await _redisService.Db.StringBitCountAsync(key);
    }

    public async Task AddUniquePlayerAsync(string playerId)
    {
        var player = await _playerService.GetPlayerAsync(playerId);

        if (player == null)
        {
            return;
        }

        await _redisService.Db.HyperLogLogAddAsync(UniquePlayersKey, playerId);
    }

    public async Task<long> GetUniquePlayerCountAsync()
    {
        return await _redisService.Db.HyperLogLogLengthAsync(UniquePlayersKey);
    }
}
