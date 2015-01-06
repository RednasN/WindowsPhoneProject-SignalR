using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPSignalR.Models;

namespace WPSignalR
{
    interface IConnection
    {
        void sendMessage(Message message);
        string getMyUserId();

        ObservableCollection<Conversation> getConversations();

    }
}
