using System.Collections.ObjectModel;
using WPSignalR.Models;

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