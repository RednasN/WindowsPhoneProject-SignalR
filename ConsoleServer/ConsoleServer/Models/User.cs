using System;

namespace ConsoleServer.Models
{
    public class User
    {
        public String userId { get; set; }
        public Location lastLocation { get; set; }

        /// <summary>
        /// User constructor, a userId is required to create a instance.
        /// </summary>
        /// <param name="userId">The user's id</param>
        public User(string userId)
        {
            this.userId = userId;
        }
        /// <summary>
        /// User constructor.
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <param name="lastLocation">The users last known location</param>
        public User(string userId, Location lastLocation)
        {
            this.userId = userId;
            this.lastLocation = lastLocation;
        }
    }
}