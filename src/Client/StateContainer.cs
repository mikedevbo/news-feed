using System.Reflection.Metadata.Ecma335;

public class StateContainer
{
    private int selectedUserId;

    private string selectedAccountIdDownloadNewTweets = "";

    public int AccountId { get; set; } = 1;

    public int SelectedUserId
    {
        get => selectedUserId;
        set
        {
            selectedUserId = value;
            this.OnSelectedUserIdChange?.Invoke(this.selectedUserId);
        }
    }

    public string SelectedAccountIdDownloadNewTweets
    {
        get => selectedAccountIdDownloadNewTweets;
        set
        {
            selectedAccountIdDownloadNewTweets = value;
            this.OnSelectedAccountIdDownloadNewTweetsChange?.Invoke(this.selectedAccountIdDownloadNewTweets);
        }
    }

    public event Action<int>? OnSelectedUserIdChange;

    public event Action<string>? OnSelectedAccountIdDownloadNewTweetsChange;
}
