namespace ChatAPI.PL.Hubs
{
    public interface IChatClient
    {
        Task ReceiveFromServer(string message);
        Task ReceiveFromUser(string username, string message);
    }
}