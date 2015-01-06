
using ConsoleServer.Managers;
using ConsoleServer.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using SignalR.Hosting.Self;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ConsoleServer
{
	class Program
	{
		static void Main(string[] args)
		{
			GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(10);

			// This will *ONLY* bind to localhost, if you want to bind to all addresses
			// use http://*:8080 to bind to all addresses. 
			// See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
			// for more information.

            string url = "http://" + getMyLocalIP() + ":8080"; 

			//169.254.80.80
			using (WebApp.Start(url))
			{
				Console.WriteLine("Server running on {0}", url);
				while (true)
				{
					string key = Console.ReadLine();
					if (key.ToUpper() == "W")
					{
						IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
						hubContext.Clients.All.addMessage("server", "ServerMessage");
						Console.WriteLine("Server Sending addMessage\n");
					}
					if (key.ToUpper() == "E")
					{
						IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
						//hubContext.Clients.All.heartbeat();

						Location location = new Location("Je moedertje", 1, 2);

						hubContext.Clients.All.JeMoeder(location);

						Console.WriteLine("Server Sending heartbeat\n");
					}
					if (key.ToUpper() == "R")
					{
						IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

						var vv = new HelloModel { Age = 37, Molly = "pushed direct from Server " };

						hubContext.Clients.All.sendHelloObject(vv);
						Console.WriteLine("Server Sending sendHelloObject\n");
					}
					if (key.ToUpper() == "C")
					{
						break;
					}
				}

				Console.ReadLine();
			}
		}

        private static string getMyLocalIP()
        {
            // Determine your ip
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
	}
	class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
		}
	}
	public class MyHub : Hub
	{
        UserManager userManager = UserManager.getUserManager();

		/// <summary>
		/// Updates the location of a specific user.
		/// </summary>
		/// <param name="location">Location of the user.</param>
		public void SendLocation(Location location)
		{			
			userManager.updateLocation(location, Context.ConnectionId);

			User currentUser = userManager.getUserById(Context.ConnectionId);

			if(currentUser != null)
			{
				Clients.Caller.GetAvailableClients(userManager.getAvailableUsers(currentUser));
			}
		}

		public void sendUserName(String userName)
		{
			userManager.UpdateUser(userName, Context.ConnectionId);
		}

		/// <summary>
		/// Sends a message to another user.
		/// </summary>
		/// <param name="newMessage">Message from user.</param>
		public void SendMessage(Message newMessage)
		{
            Console.WriteLine("Message sent to "+ newMessage.receiverId +": '"+ newMessage.text +"'\n");
			User currentUser = userManager.getUserById(newMessage.receiverId);			
			if(currentUser != null)
			{
				//newMessage.senderId = newMessage.receiverId;
				Clients.Client(Context.ConnectionId).SendMessageToUser(newMessage);
			}
		}


		public override Task OnConnected()
		{
			Console.WriteLine("Hub OnConnected {0}\n", Context.ConnectionId);

            User newUser = new User(Context.ConnectionId);
			userManager.addUser(newUser);

			return (base.OnConnected());
		}		

		public override Task OnReconnected()
		{
			Console.WriteLine("Hub OnReconnected {0}\n", Context.ConnectionId);
			return (base.OnDisconnected(true));
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			Console.WriteLine("Hub OnDisconnected {0}\n", Context.ConnectionId);

			userManager.deleteUser(Context.ConnectionId); //TODO, enable disable.

			Clients.All.SendDisconnectedUser(Context.ConnectionId);
			
			return base.OnDisconnected(stopCalled);
		}
	}

	public class HelloModel
	{
		public string Molly { get; set; }

		public int Age { get; set; }
	}
}