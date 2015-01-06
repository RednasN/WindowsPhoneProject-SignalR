using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace WPSignalR.Models
{
	public class Conversation : INotifyPropertyChanged
	{
		public Conversation(string userId)
		{
			this.userId = userId;
		}
		public string userId { get; set; }
		private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
		public ObservableCollection<Message> messages
		{
			get
			{
				return this._messages;
			}
			set
			{
				this._messages = value;
				NotifyPropertyChanged("messages");
			}
		}


		public async void addMessage(Message message)
		{
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
}
