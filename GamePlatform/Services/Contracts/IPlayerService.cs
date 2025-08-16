using GamePlatform.Models;

namespace GamePlatform.Services.Contracts;

public interface IPlayerService
{
    public Task CreateOrUpdatePlayerAsync(Player player);

    public Task<Player?> GetPlayerAsync(string playerId);

    public Task<PlayerStats> GetStatsAsync(string playerId);
}
