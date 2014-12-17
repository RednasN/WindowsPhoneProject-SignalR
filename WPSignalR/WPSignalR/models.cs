using System;
using System.Collections.Generic;

namespace WPSignalR
{
    class Location
    {
        private string userId { get; set; }
        private double latitude { get; set; }
        private double longitude { get; set; }
    }

    class Conversation
    {
        private string userId { get; set; }
		private List<Message> messages { get; set; }

        public void addMessage(Message message) {
            messages.Add(message);
        }
    }

    class Message
    {
        private string senderId { get; set; }
        private string receiverId { get; set; }
        private string text;
    }
}
