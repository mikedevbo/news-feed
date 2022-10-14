using NewsFeed.Shared.Twitter.Dto;

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

public record GroupSelected(Group Group) : IMessage;

public record UserSelected(User User) : IMessage;
