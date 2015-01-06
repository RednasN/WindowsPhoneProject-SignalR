using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using System.Diagnostics;

namespace WPSignalR
{
    public class Conversation : INotifyPropertyChanged
    {
        public string userId { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
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
        
        /// <summary>
        /// Conversation constructor, a userId is required to create a instance.
        /// </summary>
        /// <param name="userId">The userId of the user you are chatting with.</param>
        public Conversation(string userId)
        {
            this.userId = userId;
        }

		public String userName { get; set; }

        /// <summary>
        /// Adds a message to this conversation.
        /// </summary>
        /// <param name="message">The message to be added.</param>
        public async void addMessage(Message message)
        {
            // Determine if the message is incoming. If so, it should be aligned to the right.
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
                // Try to add the message to this conversation. The ListView observes this MessageCollection so do it Async to prevent errors.
                CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    this._messages.Add(message); });

                // Notify the observers about the new message
                NotifyPropertyChanged("messages");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to add message to conversation, Exception message: "+ex.Message.ToString());
            }
        }
       
        /// <summary>
        /// Notifies the observer if the PropertyChangedEventHandler is set.
        /// </summary>
        /// <param name="propertyName">The name of the property that is changed.</param>
        protected async void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); });
            }


        }

    }
}
