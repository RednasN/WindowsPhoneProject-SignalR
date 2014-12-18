using System;
using System.Collections.Generic;

namespace WPSignalR
{
    public class Location
    {
        public Location(string userId, double latitude, double longitude)
        {
            this.userId = userId;
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public string userId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Conversation
    {
        public Conversation(string userId)
        {
            this.userId = userId;
        }
        private string userId { get; set; }
        private List<Message> messages { get; set; }

        public void addMessage(Message message)
        {
            messages.Add(message);
        }
    }

    public class Message
    {
        public Message(string senderId, string receiverId, string text)
        {
            this.senderId = senderId;
            this.receiverId = receiverId;
            this.text = text;
        }
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public string text;
    }
}
