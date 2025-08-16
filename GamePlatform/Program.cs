
using GamePlatform.Services;
using GamePlatform.Services.Contracts;

namespace GamePlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IRedisService, RedisService>();
            builder.Services.AddSingleton<ILeaderboardService, LeaderboardService>();
            builder.Services.AddSingleton<IRateLimiterService, RateLimiterService>();
            builder.Services.AddSingleton<IChatService, ChatService>();
            builder.Services.AddSingleton<IPlayerService, PlayerService>();
            builder.Services.AddSingleton<IActivityService, ActivityService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();

            app.Urls.Add("http://0.0.0.0:80");

            app.Run();
        }
    }
}
