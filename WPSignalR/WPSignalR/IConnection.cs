using System.Collections.ObjectModel;

namespace WPSignalR
{
    interface IConnection
    {
        void sendMessage(Message message);
        void registerUserName(string username);
        string getMyUserId();
        ObservableCollection<Conversation> conversations { get; set; }
    }
}
