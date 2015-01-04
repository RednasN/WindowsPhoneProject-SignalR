using ConsoleServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Managers
{
	public class UserManager
	{
		private DistanceManager distanceManager = new DistanceManager();
		private List<User> availableUsers = new List<User>();

		public List<User> getAvailableUsers(User currentUser)
		{
			List<User> chatUsers = new List<User>();

			foreach(User user in availableUsers.Where( x => x.userId != currentUser.userId && x.lastLocation != null))
			{
				if(distanceManager.getDistanceInKilometers(currentUser.lastLocation.latitude, currentUser.lastLocation.longitude,
															user.lastLocation.latitude, currentUser.lastLocation.longitude) < 1)
				{
					chatUsers.Add(user);
				}
			}
			return chatUsers;
		}

		public void updateLocation(Location location, string userId)
		{
			int userIndex = availableUsers.FindIndex(x => x.userId == userId);

			if (userIndex != -1)
			{
				availableUsers[userIndex].lastLocation = location;
			}
		}

		public User getUserById(String connectionId)
		{
			return availableUsers.Find(x => x.userId == connectionId);
		}

		public void addUser(String userId)
		{
			User user = new User();
			user.userId = userId;
			availableUsers.Add(user);
		}
		
		public void addUser(User newUser)
		{
			availableUsers.Add(newUser);
		}

		static UserManager userManager;
		public static UserManager getUserManager()
		{
			if(userManager == null)
			{
				userManager = new UserManager();
				
			}
			return userManager;
		}
		

	}
}
