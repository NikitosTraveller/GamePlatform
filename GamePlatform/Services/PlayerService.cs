using GamePlatform.Models;
using GamePlatform.Services.Contracts;
using StackExchange.Redis;

namespace GamePlatform.Services;

public class PlayerService : IPlayerService
{
    private readonly IRedisService _redisService;
    private const string PlayerKeyPrefix = "player:";
    private const string PlayerStatsPrefix = "player:stats:";

    public PlayerService(IRedisService redisService) => _redisService = redisService;

    // Store player profile (Hash)
    public async Task CreateOrUpdatePlayerAsync(Player player)
    {
        var key = PlayerKeyPrefix + player.Id;
        var entries = new HashEntry[]
        {
            new("Name", player.Name),
            new("Level", player.Level)
        };
        await _redisService.Db.HashSetAsync(key, entries);
    }

    public async Task<Player?> GetPlayerAsync(string playerId)
    {
        var key = PlayerKeyPrefix + playerId;
        if (!await _redisService.Db.KeyExistsAsync(key)) return null;

        var entries = await _redisService.Db.HashGetAllAsync(key);
        return new Player
        {
            Id = playerId,
            Name = entries.FirstOrDefault(e => e.Name == "Name").Value,
            Level = (int)entries.FirstOrDefault(e => e.Name == "Level").Value
        };
    }

    // Player stats (with caching)
    public async Task<PlayerStats> GetStatsAsync(string playerId)
    {
        var key = PlayerStatsPrefix + playerId;
        var cached = await _redisService.Db.StringGetAsync(key);
        if (cached.HasValue)
            return System.Text.Json.JsonSerializer.Deserialize<PlayerStats>(cached!)!;

        // Simulate calculation
        var stats = new PlayerStats { GamesPlayed = 10, Wins = 6, Losses = 4 };
        await _redisService.Db.StringSetAsync(key, System.Text.Json.JsonSerializer.Serialize(stats), TimeSpan.FromMinutes(5));
        return stats;
    }
}
