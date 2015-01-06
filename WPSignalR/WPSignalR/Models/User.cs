using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPSignalR.Models
{
	public class User
	{
		public String userId { get; set; }
		public Location lastLocation { get; set; }
	}
}
