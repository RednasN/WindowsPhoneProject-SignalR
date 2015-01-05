﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;
using System.Diagnostics;

namespace WPSignalR
{
    public class SignalRConnection : IConnection
    {
        private string userId;
        public ConversationCollection conversations;
        private HubConnection hubConnection;
        private Task locationSender;
        const string serverIp = "192.168.1.134";
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
            conversations = new ConversationCollection();

            hubConnection = new HubConnection("http://" + serverIp + ":" + serverPort + "/");

            // Make a HubProxy to define methods on this client which then can be called by the server
            myHubProxy = hubConnection.CreateHubProxy("MyHub");

            // Define methods that can be called by the server
            // Save a message upon receiving one
            myHubProxy.On<Message>("sendMessageToUser", (message) => saveMessage(message));

            //myHubProxy.On<Location>("sendHelloObject", hello => OnSendData(""));

            myHubProxy.On<Location>("jeMoeder", hello => Debug.WriteLine("Recieved je moeder location {0}, {1} \n", hello.userId, hello.latitude));

            //myHubProxy.On<Location>("sendLocation", (location) => sendLocation(location));

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
            myHubProxy.On<List<User>>("getAvailableClients", availableUsers => Debug.WriteLine("Received users: " + availableUsers.Count));

            sendLocation();
        }

        public void sendMessage(Message message)
        {
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
                int conversationIndex = list.FindIndex(x => x.getUserId() == message.senderId);
                if (conversationIndex == -1)
                {
                    Conversation conversation = new Conversation(message.senderId);
                    conversation.addMessage(message);
                    conversations.Add(conversation);   
                }
                else
                {
                    conversations[0].addMessage(message);
                }
            }
            catch
            {
                // @TODO: Do some proper logging.
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


        public ConversationCollection getConversations()
        {
            return conversations;
        }
    }
}
