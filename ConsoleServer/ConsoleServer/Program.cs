
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
			string url = "http://172.16.142.131:8080";

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
		public void AddMessage(string name, string message)
		{
			Console.WriteLine("Hub AddMessage {0} {1}\n", name, message);
			Clients.All.addMessageDoei(name, message);
			Heartbeat();
		}

		public void SendLocation(Location location)
		{
			//Received !! :) 

			Clients.Client(Context.ConnectionId).JeMoeder(location);
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
			return (base.OnConnected());
		}		

		public override Task OnReconnected()
		{
			Console.WriteLine("Hub OnReconnected {0}\n", Context.ConnectionId);
			return (base.OnDisconnected(true));
		}
	}

	public class HelloModel
	{
		public string Molly { get; set; }

		public int Age { get; set; }
	}
}