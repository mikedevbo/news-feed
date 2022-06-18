public class StateContainer
{
    private string selectedAccountId = "";

    public string SelectedAccountId
    {
        get => selectedAccountId;
        set
        {
            selectedAccountId = value;
            this.OnSelectedAccountIdChange?.Invoke(this.selectedAccountId);
        }
    }

    public event Action<string>? OnSelectedAccountIdChange;
}
