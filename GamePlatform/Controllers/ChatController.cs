using GamePlatform.Models;
using GamePlatform.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    // Publish a message to a channel
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] Message message)
    {
        await _chatService.PublishAsync(message.Channel, message);
        return Ok();
    }

    // Subscribe to a channel (for demo/testing purposes)
    [HttpGet("subscribe/{channel}")]
    public IActionResult Subscribe(string channel)
    {
        _chatService.Subscribe(channel, msg =>
        {
            Console.WriteLine($"[{msg.Timestamp}] {msg.Channel} - {msg.Sender}: {msg.Text}");
        });

        return Ok(new { message = $"Subscribed to channel '{channel}' - check server console for messages." });
    }
}
