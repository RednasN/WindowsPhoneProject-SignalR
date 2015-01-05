using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using System.ComponentModel;
using Windows.UI.Core;
using System.Diagnostics;
using Windows.ApplicationModel.Core;

namespace WPSignalR
{

    public class ConversationCollection : ObservableCollection<Conversation>
    {
        public ConversationCollection() : base()
        {
            Conversation conversation = new Conversation("Hendrik");
            //conversation.addMessage(new Message("Hendrik", "Edwin", "Hallo wereld!!!!!!"));
            //conversation.addMessage(new Message("Edwin", "Hendrik", "Hey Hendrik"));
            //conversation.addMessage(new Message("Edwin", "Hendrik", "Mijn naam is Edwin."));
            //conversation.addMessage(new Message("Hendrik", "Edwin", "Leuk voor je..."));
            //conversation.addMessage(new Message("Edwin", "Hendrik", "Nou... lekker aardig zeg!!"));
            Add(conversation);
        }
    }

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


        public async void addMessage(Message message) {
            if (message.senderId == this.userId)
            {
                message.position = Message.Position.Right;
            }
            else
            {
                message.position = Message.Position.Left;
            }

			try
			{
				CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
				await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				  {
					  this._messages.Add(message);

				  });
			}
			catch (Exception ex)
			{

			}

			
			
            
            NotifyPropertyChanged("messages");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async void NotifyPropertyChanged(string propertyName)
        {
			if (PropertyChanged != null)
			{

				CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
				await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				  {
					  PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

				  });
			}


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
