namespace GamePlatform.Services.Contracts;

public interface IRateLimiterService
{
    public Task<bool> IsAllowedAsync(
        string key, 
        int limit, 
        TimeSpan window);
}
