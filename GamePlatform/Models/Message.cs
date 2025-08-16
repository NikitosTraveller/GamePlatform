namespace GamePlatform.Models;

public class Message
{
    public string Channel { get; set; }
    public string Sender { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
