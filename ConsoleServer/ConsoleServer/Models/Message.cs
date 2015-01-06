using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Models
{
	public class Message
	{
		public Message(string senderId, string receiverId, string text)
		{
			this.senderId = senderId;
			this.receiverId = receiverId;
			this.text = text;
		}
		public string senderId { get; set; }
		public string receiverId { get; set; }
		public string text;
		public string userName { get; set; }

	}
}
