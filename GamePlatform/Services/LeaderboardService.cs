using GamePlatform.Services.Contracts;
using StackExchange.Redis;

namespace GamePlatform.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly IRedisService _redisService;
    private readonly IPlayerService _playerService;
    private const string LeaderboardKey = "leaderboard";

    public LeaderboardService(
        IRedisService redisService,
        IPlayerService playerService)
    {
        _redisService = redisService;
        _playerService = playerService;
    }

    public async Task AddScoreAsync(string playerId, int points)
    {
        var player = await _playerService.GetPlayerAsync(playerId);

        if (player == null)
        {
            return;
        }

        await _redisService.Db.SortedSetAddAsync(LeaderboardKey, playerId, points);
    }

    public async Task<double?> GetRankAsync(string playerId)
    {
        var player = await _playerService.GetPlayerAsync(playerId);

        if (player == null)
        {
            return 0;
        }

        return await _redisService.Db.SortedSetRankAsync(LeaderboardKey, playerId, Order.Descending);
    }

    public async Task<IEnumerable<SortedSetEntry>> GetTopAsync(int count)
    {
        return await _redisService.Db.SortedSetRangeByRankWithScoresAsync(LeaderboardKey, 0, count - 1, Order.Descending);
    }
}
