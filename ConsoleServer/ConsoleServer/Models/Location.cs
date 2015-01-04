using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Models
{
	public class Location
	{
		public Location()
		{

		}
		public Location(string userId, double latitude, double longitude)
		{
			this.userId = userId;
			this.latitude = latitude;
			this.longitude = longitude;
		}
		public string userId { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
	}
}
