using System;
using System.Collections.Generic;

namespace WPSignalR
{
    public class Location
    {
        public string userId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Conversation
    {
        public string userId { get; set; }
        public List<Message> messages { get; private set; }

        public void addMessage(Message message) {
            messages.Add(message);
        }
    }

    public class Message
    {
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public string text;
    }
}
