public class StateContainer
{
    private string selectedAccountId = "";

    private string selectedAccountIdDownloadNewTweets = "";

    public string SelectedAccountId
    {
        get => selectedAccountId;
        set
        {
            selectedAccountId = value;
            this.OnSelectedAccountIdChange?.Invoke(this.selectedAccountId);
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

    public event Action<string>? OnSelectedAccountIdChange;

    public event Action<string>? OnSelectedAccountIdDownloadNewTweetsChange;
}
