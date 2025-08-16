using GamePlatform.Models;

namespace GamePlatform.Services.Contracts;

public interface IChatService
{
    public Task PublishAsync(
        string channel, 
        Message message);

    public void Subscribe(
        string channel, 
        Action<Message> handler);
}
