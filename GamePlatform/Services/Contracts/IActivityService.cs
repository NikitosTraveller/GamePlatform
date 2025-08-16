namespace GamePlatform.Services.Contracts;

public interface IActivityService
{
    public Task MarkLoginAsync(string playerId);

    public Task<long> CountDailyActivePlayersAsync(DateTime date);

    public Task AddUniquePlayerAsync(string playerId);

    public Task<long> GetUniquePlayerCountAsync();
}
