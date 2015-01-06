using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPSignalR.Models
{
	public class Message
    {
        public Message(string senderId, string receiverId, string text)
        {
            this.senderId = senderId;
            this.receiverId = receiverId;
            this.text = text;
        }

        public enum Position
        {
            Left,
            Right
        }
        public Position position { get; set;}

        public string alignment
        {
            get
            {
                if (this.position == Position.Left)
                {
                    return "Left";
                } else {
                    return "Right";
                }
            }
        }
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public string text { get; set; }

		public string userName { get; set; }
    }
}

