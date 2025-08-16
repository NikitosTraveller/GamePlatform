using StackExchange.Redis;

namespace GamePlatform.Services.Contracts;

public interface ILeaderboardService
{
    public Task AddScoreAsync(
        string playerId, 
        int points);

    public Task<double?> GetRankAsync(string playerId);

    public Task<IEnumerable<SortedSetEntry>> GetTopAsync(int count);
}
