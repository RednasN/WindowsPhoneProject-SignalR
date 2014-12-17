using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

			var hubConnection = new HubConnection("http://172.16.142.131:8080/");
			//Make proxy to hub based on hub name on server

			IHubProxy myHubProxy = hubConnection.CreateHubProxy("MyHub");
			myHubProxy.On<string, string>("addMessage", (name, message) => Debug.WriteLine("Recieved addMessage: " + name + ": " + message + "\n"));
			myHubProxy.On("heartbeat", () => Debug.WriteLine("Recieved heartbeat \n"));
			myHubProxy.On<HelloModel>("sendHelloObject", hello => Debug.WriteLine("Recieved sendHelloObject {0}, {1} \n", hello.Molly, hello.Age));

			//WebSocketTransport transport = new WebSocketTransport();
			//Start connection
			//IClientTransport transport = new 
			hubConnection.Start().ContinueWith(task =>
			{
				if (task.IsFaulted)
				{

					Debug.WriteLine("There was an error opening the connection:{0}",
									  task.Exception.GetBaseException());
				}
				else
				{
					//Console.WriteLine("Connected");
				}

			}).Wait();
			HelloModel modelModel = new HelloModel();
			modelModel.Age = 15;
			modelModel.Molly = "Test";

			myHubProxy.Invoke("sendHelloObject", modelModel).ContinueWith(task =>
			{

			});

			myHubProxy.Invoke("addMessage", "client message", " sent from console client").ContinueWith(task =>
			{
				if (task.IsFaulted)
				{
					Debug.WriteLine("!!! There was an error opening the connection:{0} \n", task.Exception.GetBaseException());
				}

			}).Wait();

			for (int i = 0; i < 2; i++)
			{

				myHubProxy.Invoke("Heartbeat").ContinueWith(task =>
				{
					if (task.IsFaulted)
					{
						Debug.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
					}

				}).Wait(1000);
				Debug.WriteLine("client heartbeat sent to server\n");

			}
        }

		public class HelloModel
		{
			public string Molly { get; set; }

			public int Age { get; set; }
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