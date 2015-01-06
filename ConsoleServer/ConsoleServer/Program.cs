
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
			User senderUser = userManager.getUserById(Context.ConnectionId);

			newMessage.userName = senderUser.userName;
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
}