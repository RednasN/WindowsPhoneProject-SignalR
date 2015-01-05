using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPSignalR
{
    interface IConnection
    {
        void sendMessage(Message message);
        string getMyUserId();

        ConversationCollection getConversations();
    }
}
