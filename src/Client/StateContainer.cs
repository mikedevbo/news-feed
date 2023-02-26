public class StateContainer
{
    public int AccountId { get; set; } = 1;

    public event Action<IMessage>? Subscribers;

    public void Publish(IMessage message)
    {
        this.Subscribers?.Invoke(message);
    }
}

public interface IMessage {};

public record GroupSelected(int Id, string Name) : IMessage;

public record UserSelected(int Id, string Name, string TwitterUserId) : IMessage;

public record TweetsLoaded(int UserId, int TweetsCount) : IMessage;

public record DownloadTweetsRequested(int UserId, string TwitterUserId) : IMessage;
