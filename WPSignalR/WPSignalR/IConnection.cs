using System.Collections.ObjectModel;

namespace WPSignalR
{
    interface IConnection
    {
        void sendMessage(Message message);
        string getMyUserId();
        ObservableCollection<Conversation> conversations { get; set; }
    }
}
