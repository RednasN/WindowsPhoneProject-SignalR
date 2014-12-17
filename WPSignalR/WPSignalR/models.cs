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
        private string userId { get; set; }
		private List<Message> messages { get; set; }

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
