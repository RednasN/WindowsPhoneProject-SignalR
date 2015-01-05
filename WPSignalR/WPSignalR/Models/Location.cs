namespace WPSignalR
{
    public class Location
    {
        /// <summary>
        /// Location constructor, multiple params are required to create a instance.
        /// </summary>
        /// <param name="userId">The userId of the user who this Location belongs to.</param>
        /// <param name="latitude">Locations latitude</param>
        /// <param name="longitude">Locations longitude</param>
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
