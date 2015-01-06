using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;

namespace WPSignalR
{
    public class SignalRConnection : IConnection, INotifyPropertyChanged
    {
        private string userId;

        private ObservableCollection<Conversation> _conversations = new ObservableCollection<Conversation>();
        public ObservableCollection<Conversation> conversations
        {
            get
            {
                return this._conversations;
            }
            set
            {
                this._conversations = value;
                NotifyPropertyChanged("conversations");
            }
        }

		public async void RemoveConversation(int conversationIndex)
		{
			try
			{
				CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
				await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					this._conversations.RemoveAt(conversationIndex);
				});
				NotifyPropertyChanged("conversations");
			}
			catch(Exception ex)
			{

			}
		}

        public async void AddConversation(Conversation newConversation)
		{
            try
            {
                CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this._conversations.Add(newConversation);
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to add conversation to the Collection in SignalRConnection, Exception message: " + ex.Message.ToString());
            }
			NotifyPropertyChanged("conversations");
		}
        private HubConnection hubConnection;
        private Task locationSender;
        const string serverIp = "192.168.1.126";
        const string serverPort = "8080";
        const int sendLocationDelay = 5000;
        Boolean connected = false;
        IHubProxy myHubProxy;
        private static SignalRConnection instance;

        public static SignalRConnection Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new SignalRConnection();
                    instance.start();
                }
                return instance;
            }
        }

        private void start()
        {
            hubConnection = new HubConnection("http://" + serverIp + ":" + serverPort + "/");

            // Make a HubProxy to define methods on this client which then can be called by the server
            myHubProxy = hubConnection.CreateHubProxy("MyHub");

            // Define methods that can be called by the server
            // Save a message upon receiving one
            myHubProxy.On<Message>("sendMessageToUser", (message) => saveMessage(message));

            var httpClient = new DefaultHttpClient();

            // Capture ConnectionState changes
            hubConnection.StateChanged += (change) =>
            {
                switch (change.NewState)
                {
                    case ConnectionState.Connected:
                        // Start the SendLocation task
                        locationSender = Task_SendLocationAync();
                        connected = true;
                        break;
                    default:
                        // If the client is no longer connected to the server..
                        break;
                }
            };

            hubConnection.Start(new AutoTransport(httpClient,
            new IClientTransport[] 
			{ 
				new LongPollingTransport(httpClient),
				new AutoTransport(httpClient)
			}));


            while (!connected)
            {

            }
            myHubProxy.On<List<User>>("getAvailableClients", availableUsers => startConversations(availableUsers));
			myHubProxy.On<String>("sendDisconnectedUser", connectionId => removeConversation(connectionId));
            sendLocation();
        }

		public void removeConversation(String connectionId)
		{
			List<Conversation> list = conversations.ToList<Conversation>();
			int conversationToRemove = list.FindIndex(x => x.userId == connectionId);

			if(conversationToRemove != -1)
			{
				RemoveConversation(conversationToRemove);
			}
		}

        private void startConversations(List<User> availableUsers)
        {
            List<Conversation> list = conversations.ToList<Conversation>();
            foreach (User user in availableUsers)
            {
                int conversationIndex = list.FindIndex(x => x.userId == user.userId);
                if (conversationIndex == -1) {
                    AddConversation(new Conversation(user.userId));
                }
            }
        }

        public void sendMessage(Message message)
        {
			List<Conversation> list = conversations.ToList<Conversation>();

			int conversationIndex = list.FindIndex(x => x.userId == message.receiverId);

			if(conversationIndex != -1)
			{
				conversations[conversationIndex].addMessage(message);
			}
            myHubProxy.Invoke("sendMessage", message);
        }

        public String getMyUserId()
        {
            return hubConnection.ConnectionId;
        }

        private void sendLocation()
        {
            Location location = new Location("Test", 1, 2);
            myHubProxy.Invoke("sendLocation", location);
        }

        public static int convertStringToInt32(string text)
        {
            int output;
            if (Int32.TryParse(text, out output))
            {
                return output;
            }
            else
            {
                throw new Exception("Unable to convert string to int!");
            }
        }

        private void saveMessage(Message message)
        {
            try
            {
                List<Conversation> list = conversations.ToList<Conversation>();
                int conversationIndex = list.FindIndex(x => x.userId == message.senderId);
                if (conversationIndex == -1)
                {
                    Conversation conversation = new Conversation(message.senderId);
                    conversation.addMessage(message);
                    conversations.Add(conversation);
                    NotifyPropertyChanged("conversations");
                }
                else
                {
                    conversations[conversationIndex].addMessage(message);
                }
            }
            catch
            {
                // @TODO: Do some proper logging.
            }
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

        private async Task Task_SendLocationAync()
        {
            while (hubConnection.State == ConnectionState.Connected)
            {
                await Task.Delay(sendLocationDelay);
                await Task.Run(() =>
                {
                    // send location data
                    Debug.WriteLine("sending locationdata..");
                    sendLocation();
                });
            }
        }

        private Location getCurrentLocation()
        {
            Location location = new Location(this.userId, -52.1234, 12.1234);
            // @TODO: get current device location and add latitude and longitude here.
            return location;
        }
    }
}
