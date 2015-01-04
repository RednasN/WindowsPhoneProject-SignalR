using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WPSignalR
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {

        private TransitionCollection transitions;
        private HubConnection hubConnection;
        private Task locationSender;
        const string serverIp = "172.16.142.131";
        const string serverPort = "8080";
        const int sendLocationDelay = 5000;

        private string userId;
        private List<Conversation> conversations;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            this.start();
        }

		Boolean connected = false;
		IHubProxy myHubProxy;
		private void start()
		{

			hubConnection = new HubConnection("http://" + serverIp + ":" + serverPort + "/");

			// Make a HubProxy to define methods on this client which then can be called by the server
			myHubProxy = hubConnection.CreateHubProxy("MyHub");

			// Define methods that can be called by the server
			// Save a message upon receiving one
			myHubProxy.On<Message>("sendMessage", (message) => saveMessage(message));

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
						// locationSender = Task_SendLocationAync();
						//
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
                int senderId = convertStringToInt32(message.senderId);
                if (conversations[senderId] == null)
                {
                    conversations[senderId] = new Conversation(message.senderId);
                }
                conversations[senderId].addMessage(message);
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

					
                   // hubConnection.Send(getCurrentLocation());
                });
            }
        }

        private Location getCurrentLocation()
        {
            Location location = new Location(this.userId, -52.1234, 12.1234);
            // @TODO: get current device location and add latitude and longitude here.
            return location;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}