using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using System.ComponentModel;

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

    public class Conversation : INotifyPropertyChanged
    {
        public Conversation(string userId)
        {
            this.userId = userId;
        }
        private string userId;
        public string getUserId() {
            return this.userId;
        }
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> messages
        {
            get {
                return this._messages;
            }
            set
            {
                this._messages = value;
                NotifyPropertyChanged("messages");
            }            
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

            this._messages.Add(message);
            NotifyPropertyChanged("messages");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
