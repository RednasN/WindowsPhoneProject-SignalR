using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

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
        private string userId;
        public string getUserId() {
            return this.userId;
        }
        private List<Message> messages = new List<Message>();
        public List<Message> getMessages()
        {
            return this.messages;
        }
        public void addMessage(Message message) {
            if (message.senderId == this.userId)
            {
                message.position = Message.Position.Right;
            }
            else
            {
                message.position = Message.Position.Left;
            }
            this.messages.Add(message);
        }
    }

	public class User
	{
		public String userId { get; set; }
		public Location lastLocation { get; set; }
	}

    public class Message
    {
        public Message(string senderId, string receiverId, string text)
        {
            this.senderId = senderId;
            this.receiverId = receiverId;
            this.text = text;
        }
        public enum Position
        {
            Left,
            Right
        }
        public Position position { get; set;}

        public string alignment
        {
            get
            {
                if (this.position == Position.Left)
                {
                    return "Left";
                } else {
                    return "Right";
                }
            }
        }
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public string text { get; set; }
    }
}
