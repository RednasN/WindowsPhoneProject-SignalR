﻿
using ConsoleServer.Managers;
using ConsoleServer.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using SignalR.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer
{
	class Program
	{
		static void Main(string[] args)
		{
			// This will *ONLY* bind to localhost, if you want to bind to all addresses
			// use http://*:8080 to bind to all addresses. 
			// See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
			// for more information.
			string url = "http://192.168.1.126:8080";

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

						Location location = new Location();
						location.latitude = 1;
						location.latitude = 2;
						location.userId = "Je moedertje";

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



		public void AddMessage(string name, string message)
		{
			Console.WriteLine("Hub AddMessage {0} {1}\n", name, message);
			Clients.All.addMessageDoei(name, message);
			Heartbeat();
		}

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

		/// <summary>
		/// Sends a message to another user.
		/// </summary>
		/// <param name="newMessage">Message from user.</param>
		public void SendMessage(Message newMessage)
		{
			User currentUser = userManager.getUserById(newMessage.receiverId);
			newMessage.userName = currentUser.userName;
			if(currentUser != null)
			{
				//newMessage.senderId = newMessage.receiverId;
				Clients.Client(newMessage.receiverId).SendMessageToUser(newMessage);
			}
		}
		public void sendUserName(String userName)
		{
			userManager.UpdateUser(userName, Context.ConnectionId);
		}



		public void Heartbeat()
		{
			Clients.All.heartbeat();
		}

		public void SendHelloObject(HelloModel hello)
		{
			Console.WriteLine("Hub hello {0} {1}\n", hello.Molly, hello.Age);
			AddMessage("Doei", "Goeie dag");
		}

		public override Task OnConnected()
		{
			Console.WriteLine("Hub OnConnected {0}\n", Context.ConnectionId);

			User newUser = new User();
			newUser.userId = Context.ConnectionId;
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

			//userManager.deleteUser(Context.ConnectionId); //TODO, enable disable.

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